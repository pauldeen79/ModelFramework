namespace ModelFramework.Objects.Extensions;

public static partial class TypeBaseExtensions
{
    public const string NullCheckMetadataValue = "ImmutableClass.Constructor.NullCheck";

    public static IClass ToImmutableClass(this ITypeBase instance, ImmutableClassSettings settings)
        => instance.ToImmutableClassBuilder(settings).BuildTyped();

    public static ClassBuilder ToImmutableClassBuilder(this ITypeBase instance, ImmutableClassSettings settings)
    {
        if (!settings.AllowGenerationWithoutProperties && !instance.Properties.Any() && !settings.InheritanceSettings.EnableInheritance)
        {
            throw new InvalidOperationException("To create an immutable class, there must be at least one property");
        }

        return new ClassBuilder()
            .WithName(settings.ConstructorSettings.ValidateArguments == ArgumentValidationType.Shared
                ? $"{instance.Name}Base"
                : instance.Name)
            .WithNamespace(instance.Namespace)
            .WithBaseClass(GetImmutableClassBaseClass(instance, settings))
            .WithAbstract(settings.InheritanceSettings.EnableInheritance && (settings.InheritanceSettings.BaseClass == null || settings.InheritanceSettings.IsAbstract))
            .AddProperties(CreateImmutableClassProperties(instance, settings))
            .AddConstructors(CreateImmutableClassConstructor(instance, settings))
            .AddMethods(GetImmutableClassMethods(instance, settings, false))
            .AddInterfaces
            (
                settings.ImplementIEquatable
                    ? new[] { $"IEquatable<{instance.Name}>" }
                    : Enumerable.Empty<string>()
            )
            .AddInterfaces
            (
                settings.InheritanceSettings.InheritFromInterfaces
                    ? new[] { FormatInstanceName(instance, false, settings.InheritanceSettings.FormatInstanceTypeNameDelegate) }
                    : Enumerable.Empty<string>()
            )
            .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)))
            .AddFields(instance.Properties.SelectMany(x => x.Metadata.GetValues<IClassField>(MetadataNames.CustomImmutableBackingField).Select(x => new ClassFieldBuilder(x))))
            .AddGenericTypeArguments(instance.GenericTypeArguments)
            .AddGenericTypeArgumentConstraints(instance.GenericTypeArgumentConstraints);
    }

    public static ClassBuilder ToImmutableClassValidateOverrideBuilder(this ITypeBase instance, ImmutableClassSettings settings)
    {
        if (!settings.AllowGenerationWithoutProperties && !instance.Properties.Any() && !settings.InheritanceSettings.EnableInheritance)
        {
            throw new InvalidOperationException("To create an immutable class, there must be at least one property");
        }

        if (settings.ConstructorSettings.ValidateArguments != ArgumentValidationType.Shared)
        {
            throw new InvalidOperationException("Can't create an validate override class for ArgumentValidationType other than Optional");
        }

        return new ClassBuilder()
            .WithName(instance.Name)
            .WithNamespace(instance.Namespace)
            .WithBaseClass(instance.GenericTypeArguments.Any()
                ? $"{instance.Name}Base<{string.Join(", ", instance.GenericTypeArguments)}>"
                : $"{instance.Name}Base")
            .AddConstructors
            (
                new ClassConstructorBuilder()
                    .AddParameter("original", instance.GenericTypeArguments.Any()
                        ? $"{instance.Name}<{string.Join(", ", instance.GenericTypeArguments)}>"
                        : instance.Name)
                    .WithChainCall(instance.GenericTypeArguments.Any()
                        ? $"base(({instance.Name}Base<{string.Join(", ", instance.GenericTypeArguments)}>)original)"
                        : $"base(({instance.Name}Base)original)"),
                new ClassConstructorBuilder()
                    .AddParameters(CreateImmutableClassCtorParameters(instance, settings))
                    .AddLiteralCodeStatements
                    (
                        instance.Properties
                            .Where(p => settings.ConstructorSettings.AddNullChecks && p.Metadata.GetValue(NullCheckMetadataValue, () => !p.IsNullable && !p.IsValueType))
                            .Select
                            (
                                p => @$"if ({p.Name.ToPascalCase().GetCsharpFriendlyName()} == null) throw new {typeof(ArgumentNullException).FullName}(""{p.Name.ToPascalCase()}"");"
                            )
                    )
                    .AddLiteralCodeStatements(CreateValidationCode(instance, settings, false))
                    .WithChainCall(GenerateImmutableClassChainCall(instance, settings, true))
            )
            .AddGenericTypeArguments(instance.GenericTypeArguments)
            .AddGenericTypeArgumentConstraints(instance.GenericTypeArgumentConstraints);
    }

    private static ClassConstructorBuilder CreateImmutableClassConstructor(
        ITypeBase instance,
        ImmutableClassSettings settings)
        => new ClassConstructorBuilder()
            .WithProtected(settings.InheritanceSettings.EnableInheritance && (settings.InheritanceSettings.BaseClass == null || settings.InheritanceSettings.IsAbstract))
            .AddParameters(CreateImmutableClassCtorParameters(instance, settings))
            .AddLiteralCodeStatements
            (
                instance.Properties
                    .Where(p => instance.IsMemberValidForImmutableBuilderClass(p, settings.InheritanceSettings))
                    .Where(p => settings.ConstructorSettings.AddNullChecks && p.Metadata.GetValue(NullCheckMetadataValue, () => !p.IsNullable && !p.IsValueType))
                    .Select
                    (
                        p => @$"if ({p.Name.ToPascalCase().GetCsharpFriendlyName()} == null) throw new {typeof(ArgumentNullException).FullName}(""{p.Name.ToPascalCase()}"");"
                    )
            )
            .AddLiteralCodeStatements
            (
                instance.Properties
                    .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings))
                    .Select
                    (
                        p => string.Format
                        (
                            GetFormatStringForInitialization(p, settings),
                            p.Name.ToPascalCase().GetCsharpFriendlyName(),             // 0
                            p.TypeName.GetGenericArguments(),                          // 1
                            settings.EnableNullableReferenceTypes ? "!" : string.Empty // 2
                        )
                    )
            )
            .AddLiteralCodeStatements(CreateValidationCode(instance, settings, true))
            .WithChainCall(GenerateImmutableClassChainCall(instance, settings, false));

    private static IEnumerable<ClassPropertyBuilder> CreateImmutableClassProperties(
        ITypeBase instance,
        ImmutableClassSettings settings)
        => instance
            .Properties
            .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings))
            .Select
            (
                p => new ClassPropertyBuilder()
                    .WithName(p.Name)
                    .WithTypeName(p.TypeName.FixCollectionTypeName(settings.NewCollectionTypeName))
                    .WithStatic(p.Static)
                    .WithVirtual(p.Virtual)
                    .WithAbstract(p.Abstract)
                    .WithProtected(p.Protected)
                    .WithOverride(p.Override)
                    .WithHasGetter(p.HasGetter)
                    .WithHasInitializer(p.HasInitializer)
                    .WithHasSetter(p.Metadata.GetBooleanValue(MetadataNames.CustomImmutableHasSetter, settings.AddPrivateSetters))
                    .WithIsNullable(p.IsNullable)
                    .WithIsValueType(p.IsValueType)
                    .WithVisibility(p.Visibility)
                    .WithGetterVisibility(p.GetterVisibility)
                    .WithSetterVisibility(p.Metadata.GetBooleanValue(MetadataNames.CustomImmutableHasSetter, settings.AddPrivateSetters)
                        ? Visibility.Private
                        : p.SetterVisibility)
                    .WithInitializerVisibility(p.InitializerVisibility)
                    .WithExplicitInterfaceName(p.ExplicitInterfaceName)
                    .AddMetadata
                    (
                        p.Metadata
                            .Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName))
                            .Select(x => new MetadataBuilder(x))
                    )
                    .AddAttributes(p.Attributes.Select(x => new AttributeBuilder(x)))
                    .AddGetterCodeStatements(p.Metadata.GetValues<ICodeStatement>(MetadataNames.CustomImmutablePropertyGetterStatement).Select(x => x.CreateBuilder()).WhenEmpty(() => p.GetterCodeStatements.Select(x => x.CreateBuilder())))
                    .AddSetterCodeStatements(p.Metadata.GetValues<ICodeStatement>(MetadataNames.CustomImmutablePropertySetterStatement).Select(x => x.CreateBuilder()).WhenEmpty(() => p.SetterCodeStatements.Select(x => x.CreateBuilder())))
                    .AddInitializerCodeStatements(p.InitializerCodeStatements.Select(x => x.CreateBuilder()))
            );

    private static IEnumerable<ParameterBuilder> CreateImmutableClassCtorParameters(
        ITypeBase instance,
        ImmutableClassSettings settings)
        => instance.Properties
            .Select
            (
                p => new ParameterBuilder()
                    .WithName(p.Name.ToPascalCase())
                    .WithTypeName(string.Format
                    (
                        p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName))
                            .GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixCollectionTypeName(settings.ConstructorSettings.CollectionTypeName.WhenNullOrEmpty(typeof(IEnumerable<>).WithoutGenerics()))),
                        p.Name.ToPascalCase().GetCsharpFriendlyName(),
                        p.TypeName.GetGenericArguments()
                    ))
                    .WithIsNullable(p.IsNullable)
                    .WithIsValueType(p.IsValueType)
            );

    private static string[] CreateValidationCode(this ITypeBase instance, ImmutableClassSettings settings, bool baseClass)
        => settings.AddValidationCode switch
        {
            ArgumentValidationType.DomainOnly =>
                new[]
                {
                    string.Format
                    (
                        instance.Metadata.GetStringValue(MetadataNames.CustomValidateCode, "System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);"),
                        instance.GetFullName(), // 0
                        instance.Name,          // 1
                        instance.Namespace      // 2
                    )
                },
            ArgumentValidationType.Shared =>
                baseClass
                    ? Array.Empty<string>()
                    : new[]
                    {
                        string.Format
                        (
                            instance.Metadata.GetStringValue(MetadataNames.CustomValidateCode, "System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);"),
                            instance.GetFullName(), // 0
                            instance.Name,          // 1
                            instance.Namespace      // 2
                        )
                    },
            ArgumentValidationType.None => Array.Empty<string>(),
            _ => throw new ArgumentOutOfRangeException(nameof(settings), $"Unsupported ArgumentValidationType: {settings.AddValidationCode}")
        };

    private static string CreateImmutableClassCtorParameterNames(
        ITypeBase instance,
        ImmutableClassSettings settings)
        => string.Join(", ", CreateImmutableClassCtorParameters(instance, settings).Select(x => x.Name.ToString().GetCsharpFriendlyName()));

    private static string GetFormatStringForInitialization(IClassProperty p, ImmutableClassSettings settings)
        => p.Metadata.GetStringValue(MetadataNames.CustomImmutableConstructorInitialization,
            () => $"this.{p.Name.GetCsharpFriendlyName()} = {p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetStringValue(MetadataNames.CustomImmutableDefaultValue, () => GetDefaultFormatStringForInitialization(p, settings))};");

    private static string GetDefaultFormatStringForInitialization(IClassProperty p, ImmutableClassSettings settings)
        => p.TypeName.IsCollectionTypeName()
        && settings.ConstructorSettings.CollectionTypeName.In(typeof(IEnumerable<>).WithoutGenerics(), string.Empty)
        && settings.NewCollectionTypeName != "System.Collections.Immutable.IImmutableList"
            ? GetCollectionFormatStringForInitialization(p)
            : p.Name.ToPascalCase().GetCsharpFriendlyName();

    private static string GetCollectionFormatStringForInitialization(IClassProperty property)
        => property.IsNullable
            ? $"{property.Name.ToPascalCase()} == null ? null : new {typeof(List<>).WithoutGenerics()}<{property.TypeName.GetGenericArguments()}>({property.Name.ToPascalCase().GetCsharpFriendlyName()})"
            : $"new {typeof(List<>).WithoutGenerics()}<{property.TypeName.GetGenericArguments()}>({property.Name.ToPascalCase().GetCsharpFriendlyName()})";

    private static string GetImmutableClassBaseClass(ITypeBase instance, ImmutableClassSettings settings)
        => settings.InheritanceSettings.EnableInheritance && settings.InheritanceSettings.BaseClass != null
            ? settings.InheritanceSettings.BaseClass.GetFullName()
            : instance.GetCustomValueForInheritedClass(settings, cls => cls.BaseClass);

    private static string GenerateImmutableClassChainCall(ITypeBase instance, ImmutableClassSettings settings, bool baseClass)
    {
        if (baseClass && settings.AddValidationCode == ArgumentValidationType.Shared)
        {
            return $"base({CreateImmutableClassCtorParameterNames(instance, settings)})";
        }

        return settings.InheritanceSettings.EnableInheritance && settings.InheritanceSettings.BaseClass != null
            ? $"base({GetPropertyNamesConcatenated(settings.InheritanceSettings.BaseClass.Properties)})"
            : instance.GetCustomValueForInheritedClass(settings, cls =>
                $"base({GetPropertyNamesConcatenated(instance.Properties.Where(x => x.ParentTypeFullName == cls.BaseClass))})");
    }

    public static IClass ToImmutableExtensionClass(this ITypeBase instance, ImmutableClassExtensionsSettings settings)
        => instance.ToImmutableExtensionClassBuilder(settings).BuildTyped();

    public static ClassBuilder ToImmutableExtensionClassBuilder(this ITypeBase instance,
                                                                ImmutableClassExtensionsSettings settings)
    {
        if (!settings.AllowGenerationWithoutProperties && !instance.Properties.Any())
        {
            throw new InvalidOperationException("To create an immutable extensions class, there must be at least one property");
        }

        return new ClassBuilder()
            .WithName(instance.Name + "Extensions")
            .WithNamespace(instance.Namespace)
            .AddMethods(GetImmutableClassMethods
            (
                instance,
                settings: new ImmutableClassSettings(
                    settings.NewCollectionTypeName,
                    createWithMethod: true,
                    implementIEquatable: false),
                extensionMethod: true
            ))
            .WithStatic();
    }

    private static IEnumerable<ClassMethodBuilder> GetImmutableClassMethods(ITypeBase instance,
                                                                            ImmutableClassSettings settings,
                                                                            bool extensionMethod)
    {
        if (settings.CreateWithMethod)
        {
            // note that we don't use a filter for inherited types here... Just generate a full with statement
            yield return
                new ClassMethodBuilder()
                    .WithName("With")
                    .WithTypeName(instance.Name)
                    .WithStatic(extensionMethod)
                    .WithExtensionMethod(extensionMethod)
                    .AddLiteralCodeStatements($"return new {instance.Name}",
                                               "(")
                    .AddLiteralCodeStatements
                    (
                        instance
                            .Properties
                            .Select
                            (p => new
                            {
                                p.Name,
                                TypeName = p.TypeName.FixCollectionTypeName(settings.NewCollectionTypeName),
                                OriginalMetadata = p.Metadata,
                                Metadata = p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)),
                                Suffix = p.Name != instance.Properties.Last().Name
                                    ? ","
                                    : string.Empty
                            }
                            )
                            .Select(p => $"    {p.Name.ToPascalCase()} == default({string.Format(p.Metadata.GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName), p.TypeName).GetCsharpFriendlyTypeName()}) ? {GetInstanceName(extensionMethod)}.{p.Name} : {string.Format(p.OriginalMetadata.GetStringValue(MetadataNames.CustomBuilderDefaultValue, p.Name.ToPascalCase()), p.Name.ToPascalCase())}{p.Suffix}")
                    )
                    .AddLiteralCodeStatements(");")
                    .AddParameters
                    (
                        extensionMethod
                            ? new[] { new ParameterBuilder().WithName("instance").WithTypeName(instance.Name) }
                            : Enumerable.Empty<ParameterBuilder>()
                    )
                    .AddParameters
                    (
                        instance
                            .Properties
                            .Select
                            (
                                p => new ParameterBuilder()
                                    .WithName(p.Name.ToPascalCase().GetCsharpFriendlyName())
                                    .WithTypeName(string.Format
                                    (
                                        p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetStringValue
                                        (
                                            MetadataNames.CustomImmutableArgumentType,
                                            p.TypeName.FixCollectionTypeName(settings.NewCollectionTypeName)
                                        ), p.TypeName.FixCollectionTypeName(settings.NewCollectionTypeName)
                                    ).GetCsharpFriendlyTypeName())
                                    .WithDefaultValue(new Literal($"default({p.Metadata.GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixCollectionTypeName(settings.NewCollectionTypeName)).GetCsharpFriendlyTypeName()})"))
                            )
                    );
        }

        if (settings.ImplementIEquatable)
        {
            // note that we don't use a filter for inherited types here... Just generate a full IEquatable method
            yield return new ClassMethodBuilder()
                .WithName("Equals")
                .WithType(typeof(bool))
                .WithOverride()
                .AddParameters
                (
                    new ParameterBuilder().WithName("obj").WithType(typeof(object))
                )
                .AddLiteralCodeStatements
                (
                    $"return Equals(obj as {instance.Name});"
                );
            yield return new ClassMethodBuilder()
                .WithName($"IEquatable<{instance.Name}>.Equals")
                .WithType(typeof(bool))
                .AddParameters
                (
                    new ParameterBuilder().WithName("other").WithTypeName(instance.Name)
                )
                .AddLiteralCodeStatements
                (
                    $"return other != null &&{Environment.NewLine}       {GetEqualsProperties(instance)};"
                );
            yield return new ClassMethodBuilder()
                .WithName("GetHashCode")
                .WithType(typeof(int))
                .WithOverride()
                .AddLiteralCodeStatements("int hashCode = 235838129;")
                .AddLiteralCodeStatements
                (
                    instance.Properties.Select(p => p.IsValueType
                        ? $"hashCode = hashCode * -1521134295 + {p.Name}.GetHashCode();"
                        : $"hashCode = hashCode * -1521134295 + EqualityComparer<{p.TypeName}>.Default.GetHashCode({p.Name});")
                )
                .AddLiteralCodeStatements("return hashCode;");
            yield return new ClassMethodBuilder()
                .WithName("==")
                .WithType(typeof(bool))
                .WithStatic()
                .WithOperator()
                .AddParameters
                (
                    new ParameterBuilder().WithName("left").WithTypeName(instance.Name),
                    new ParameterBuilder().WithName("right").WithTypeName(instance.Name)
                )
                .AddLiteralCodeStatements($"return EqualityComparer<{instance.Name}>.Default.Equals(left, right);");
            yield return new ClassMethodBuilder()
                .WithName("!=")
                .WithType(typeof(bool))
                .WithStatic()
                .WithOperator()
                .AddParameters
                (
                    new ParameterBuilder().WithName("left").WithTypeName(instance.Name),
                    new ParameterBuilder().WithName("right").WithTypeName(instance.Name)
                )
                .AddLiteralCodeStatements("return !(left == right);");
        }
    }

    private static string GetInstanceName(bool extensionMethod)
        => extensionMethod
            ? "instance"
            : "this";

    private static string GetEqualsProperties(ITypeBase instance)
        => string.Join(" &&" + Environment.NewLine + "       ",
                       instance.Properties.Select(p => $"{p.Name} == other.{p.Name}"));

    private static string GetPropertyNamesConcatenated(IEnumerable<IClassProperty> properties)
        => string.Join(", ", properties.Select(x => x.Name.ToPascalCase().GetCsharpFriendlyName()));

    private static string FormatInstanceName(ITypeBase instance,
                                             bool forCreate,
                                             Func<ITypeBase, bool, string>? formatInstanceTypeNameDelegate)
    {
        if (formatInstanceTypeNameDelegate != null)
        {
            var retVal = formatInstanceTypeNameDelegate(instance, forCreate);
            if (!string.IsNullOrEmpty(retVal))
            {
                return retVal;
            }
        }

        return instance.GetFullName().GetCsharpFriendlyTypeName();
    }
}
