﻿namespace ModelFramework.Objects.Extensions;

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
            .WithName(instance.Name)
            .WithNamespace(instance.Namespace)
            .WithBaseClass(GetImmutableClassBaseClass(instance, settings))
            .WithAbstract(settings.InheritanceSettings.EnableInheritance && (settings.InheritanceSettings.BaseClass == null || settings.InheritanceSettings.IsAbstract))
            .AddProperties
            (
                instance
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
                    )
            )
            .AddConstructors
            (
                new ClassConstructorBuilder()
                    .WithProtected(settings.InheritanceSettings.EnableInheritance && (settings.InheritanceSettings.BaseClass == null || settings.InheritanceSettings.IsAbstract))
                    .AddParameters
                    (
                        instance.Properties
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
                            )
                    )
                    .AddLiteralCodeStatements
                    (
                        instance.Properties
                            .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings))
                            .Where(p => settings.ConstructorSettings.AddNullChecks && p.Metadata.GetValue(NullCheckMetadataValue, () => !p.IsNullable && !p.IsValueType))
                            .Select
                            (
                                p => @$"if ({p.Name.ToPascalCase().GetCsharpFriendlyName()} == null) throw new System.ArgumentNullException(""{p.Name.ToPascalCase()}"");"
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
                                    p.Name.ToPascalCase().GetCsharpFriendlyName(),
                                    p.TypeName.GetGenericArguments()
                                )
                            )
                    )
                    .AddLiteralCodeStatements
                    (
                        settings.AddValidationCode
                            ? new[]
                            {
                                "System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);"
                            }
                            : Enumerable.Empty<string>()
                    )
                    .WithChainCall(GenerateImmutableClassChainCall(instance, settings))
            )
            .AddMethods(GetImmutableClassMethods(instance, settings, false))
            .AddInterfaces
            (
                settings.ImplementIEquatable
                    ? new[] { $"IEquatable<{instance.Name}>" }
                    : Enumerable.Empty<string>()
            )
            .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)))
            .AddFields(instance.Properties.SelectMany(x => x.Metadata.GetValues<IClassField>(MetadataNames.CustomImmutableBackingField).Select(x => new ClassFieldBuilder(x))));
    }

    private static string GetFormatStringForInitialization(IClassProperty p, ImmutableClassSettings settings)
        => p.Metadata.GetStringValue(MetadataNames.CustomImmutableConstructorInitialization, () => $"this.{p.Name.GetCsharpFriendlyName()} = {p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetStringValue(MetadataNames.CustomImmutableDefaultValue, () => p.TypeName.IsCollectionTypeName() && settings.ConstructorSettings.CollectionTypeName.In(typeof(IEnumerable<>).WithoutGenerics(), string.Empty) && settings.NewCollectionTypeName != "System.Collections.Immutable.IImmutableList" ? $"new {typeof(List<>).WithoutGenerics()}<{p.TypeName.GetGenericArguments()}>({p.Name.ToPascalCase().GetCsharpFriendlyName()})" : p.Name.ToPascalCase().GetCsharpFriendlyName())};");

    private static string GetImmutableClassBaseClass(ITypeBase instance, ImmutableClassSettings settings)
        => settings.InheritanceSettings.EnableInheritance && settings.InheritanceSettings.BaseClass != null
            ? settings.InheritanceSettings.BaseClass.GetFullName()
            : instance.GetCustomValueForInheritedClass(settings, cls => cls.BaseClass);

    private static string GenerateImmutableClassChainCall(ITypeBase instance, ImmutableClassSettings settings)
        => settings.InheritanceSettings.EnableInheritance && settings.InheritanceSettings.BaseClass != null
            ? $"base({GetPropertyNamesConcatenated(settings.InheritanceSettings.BaseClass.Properties)})"
            : instance.GetCustomValueForInheritedClass(settings, cls =>
                $"base({GetPropertyNamesConcatenated(instance.Properties.Where(x => x.ParentTypeFullName == cls.BaseClass))})");

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
                            .Select(p => $"    {p.Name.ToPascalCase()} == default({string.Format(p.Metadata.GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName), p.TypeName).GetCsharpFriendlyTypeName()}) ? {GetInstanceName(extensionMethod)}.{p.Name} : {string.Format(p.OriginalMetadata.GetStringValue(MetadataNames.CustomImmutableBuilderDefaultValue, p.Name.ToPascalCase()), p.Name.ToPascalCase())}{p.Suffix}")
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
}
