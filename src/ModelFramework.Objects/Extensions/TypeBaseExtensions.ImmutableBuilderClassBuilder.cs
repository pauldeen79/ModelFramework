namespace ModelFramework.Objects.Extensions;

public static partial class TypeBaseEtensions
{
    public static IClass ToImmutableBuilderClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.ToImmutableBuilderClassBuilder(settings).Build();

    public static ClassBuilder ToImmutableBuilderClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        if (!instance.Properties.Any() && !settings.InheritanceSettings.EnableEntityInheritance)
        {
            throw new InvalidOperationException("To create an immutable builder class, there must be at least one property");
        }

        return new ClassBuilder()
            .WithName(instance.Name + "Builder")
            .AddGenericTypeArguments(new[] { "TBuilder", "TEntity" }.Where(_ => settings.IsBuilderForAbstractEntity))
            .AddGenericTypeArgumentConstraints(new[] { $"where TEntity : {FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate)}" }.Where(_ => settings.IsBuilderForAbstractEntity))
            .AddGenericTypeArgumentConstraints(new[] { $"where TBuilder : {instance.Name}Builder<TBuilder, TEntity>" }.Where(_ => settings.IsBuilderForAbstractEntity))
            .WithNamespace(instance.Namespace)
            .WithBaseClass(GetImmutableBuilderClassBaseClass(instance, settings))
            .WithAbstract(settings.IsBuilderForAbstractEntity)
            .WithPartial(instance.Partial)
            .AddConstructors(GetImmutableBuilderClassConstructors(instance, settings))
            .AddMethods(GetImmutableBuilderClassMethods(instance, settings))
            .AddProperties(GetImmutableBuilderClassProperties(instance, settings))
            .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)))
            .AddFields(GetImmutableBuilderClassFields(instance, settings, false));
    }

    public static IClass ToBuilderExtensionsClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.ToBuilderExtensionsClassBuilder(settings).Build();

    public static ClassBuilder ToBuilderExtensionsClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        if (!instance.Properties.Any() && !settings.InheritanceSettings.EnableEntityInheritance)
        {
            throw new InvalidOperationException("To create a builder extensions class, there must be at least one property");
        }

        return new ClassBuilder()
            .WithName(instance.Name + "BuilderExtensions")
            .WithNamespace(instance.Namespace)
            .WithPartial()
            .WithStatic()
            .AddMethods(GetImmutableBuilderClassPropertyMethods(instance, settings, true));
    }

    public static IClass ToNonGenericImmutableBuilderClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.ToNonGenericImmutableBuilderClassBuilder(settings).Build();

    public static ClassBuilder ToNonGenericImmutableBuilderClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        if (!settings.IsAbstractBuilder)
        {
            throw new InvalidOperationException("Not possible to generate a non-generic immutable builder class for this type");
        }

        settings = settings.ForAbstractBuilder();

        return new ClassBuilder()
            .WithName(instance.Name + "Builder")
            .WithNamespace(instance.Namespace)
            .WithBaseClass(GetImmutableBuilderClassBaseClass(instance, settings))
            .WithAbstract(settings.IsBuilderForAbstractEntity)
            .WithPartial(instance.Partial)
            .AddConstructors(GetImmutableBuilderClassConstructors(instance, settings))
            .AddProperties(GetImmutableBuilderClassProperties(instance, settings))
            .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)))
            .AddFields(GetImmutableBuilderClassFields(instance, settings, false));
    }

    private static string GetImmutableBuilderClassBaseClass(ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        if (settings.InheritanceSettings.EnableEntityInheritance
            && settings.InheritanceSettings.EnableBuilderInheritance
            && settings.InheritanceSettings.BaseClass == null
            && !settings.IsForAbstractBuilder)
        {
            return instance.Name + "Builder";
        }

        if (settings.InheritanceSettings.EnableEntityInheritance
            && settings.InheritanceSettings.EnableBuilderInheritance
            && settings.InheritanceSettings.BaseClass != null
            && !settings.IsForAbstractBuilder
            && settings.InheritanceSettings.IsAbstract)
        {
            return instance.Name + "Builder";
        }

        return instance.GetCustomValueForInheritedClass(settings, cls => settings.InheritanceSettings.EnableBuilderInheritance && settings.InheritanceSettings.BaseClass != null
            ? $"{cls.BaseClass.GetClassName()}Builder"
            : $"{cls.BaseClass.GetClassName()}Builder<{instance.Name}Builder, {FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate)}>");
    }

    private static IEnumerable<ClassFieldBuilder> GetImmutableBuilderClassFields(ITypeBase instance, ImmutableBuilderClassSettings settings, bool isForWithStatement)
    {
        if (settings.IsAbstractBuilder)
        {
            yield break;
        }

        foreach (var field in instance.GetFields()
            .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement))
            .Select(x => new ClassFieldBuilder(x)))
        {
            yield return field;
        }

        if (settings.UseLazyInitialization)
        {
            foreach (var property in instance.Properties
                .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement))
                .Where(x => !x.TypeName.IsCollectionTypeName()))
            {

                yield return new ClassFieldBuilder()
                    .WithName($"_{property.Name.ToPascalCase()}Delegate")
                    .WithTypeName($"System.Lazy<{CreateLazyPropertyTypeName(property, settings)}>");
            }
        }
    }

    private static IEnumerable<ClassConstructorBuilder> GetImmutableBuilderClassConstructors(ITypeBase instance,
                                                                                             ImmutableBuilderClassSettings settings)
    {
        if (settings.IsAbstractBuilder)
        {
            yield break;
        }

        yield return new ClassConstructorBuilder()
            .WithChainCall(CreateImmutableBuilderClassConstructorChainCall(instance, settings))
            .WithProtected(settings.IsBuilderForAbstractEntity)
            .AddLiteralCodeStatements
            (
                instance.Properties
                    .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement: false))
                    .Where(x => x.TypeName.IsCollectionTypeName())
                    .Select(x => $"{x.Name} = new {GetImmutableBuilderClassConstructorInitializer(settings, x)}();")
            )
            .AddLiteralCodeStatements(settings.TypeSettings.EnableNullableReferenceTypes ? new[] { "#pragma warning disable CS8603 // Possible null reference return." } : Array.Empty<string>())
            .AddLiteralCodeStatements
            (
                instance.Properties
                    .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement: false))
                    .Where(x => settings.ConstructorSettings.SetDefaultValues
                        && !x.TypeName.IsCollectionTypeName()
                        && (!x.IsNullable || settings.UseLazyInitialization))
                    .Select(x => GenerateDefaultValueStatement(x, settings))
            )
            .AddLiteralCodeStatements(settings.TypeSettings.EnableNullableReferenceTypes ? new[] { "#pragma warning restore CS8603 // Possible null reference return." } : Array.Empty<string>());

        if (settings.ConstructorSettings.AddCopyConstructor)
        {
            yield return new ClassConstructorBuilder()
                .WithChainCall(CreateImmutableBuilderClassCopyConstructorChainCall(instance, settings))
                .WithProtected(settings.IsBuilderForAbstractEntity)
                .With(x =>
                {
                    if (settings.ConstructorSettings.AddNullChecks)
                    {
                        x.AddLiteralCodeStatements
                        (
                            "if (source == null)",
                            "{",
                            @"    throw new System.ArgumentNullException(""source"");",
                            "}"
                        );
                    }
                })
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName("source")
                        .WithTypeName(FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate))
                )
                .AddParameters
                (
                    instance.Metadata.GetValues<IParameter>(MetadataNames.AdditionalBuilderCopyConstructorAdditionalParameter)
                        .Select(x => new ParameterBuilder(x))
                )
                .AddLiteralCodeStatements
                (
                    instance.Properties
                        .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement: false))
                        .Where(x => x.TypeName.IsCollectionTypeName())
                        .Select(x => $"{x.Name} = new {GetCopyConstructorInitializeExpression(settings, x)}();")
                )
                .AddLiteralCodeStatements
                (
                    instance.Properties
                        .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement: false))
                        .Select(x => x.CreateImmutableBuilderInitializationCode(settings) + ";")
                );
        }

        if (settings.ConstructorSettings.AddConstructorWithAllProperties)
        {
            var properties = GetImmutableBuilderConstructorProperties(instance, settings, true);

            yield return new ClassConstructorBuilder()
                .AddParameters(properties.Select(p => new ParameterBuilder()
                    .WithName(p.Name.ToPascalCase())
                    .WithTypeName
                    (
                        string.Format
                        (
                            p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.TypeSettings.NewCollectionTypeName)).GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)),
                            p.Name.ToPascalCase().GetCsharpFriendlyName(),
                            p.TypeName.GetGenericArguments()
                        )
                    )
                    .WithIsNullable(p.IsNullable)
                ))
                .AddLiteralCodeStatements
                (
                    properties
                        .Where(p => p.TypeName.IsCollectionTypeName())
                        .Select(p => $"{p.Name} = new {GetConstructorInitializeExpressionForCollection(settings, p)}();")
                )
                .AddLiteralCodeStatements
                (
                    instance.Properties
                        .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement: false))
                        .Where(x => !settings.UseLazyInitialization || x.TypeName.IsCollectionTypeName())
                        .Select
                        (
                            x => x.TypeName.IsCollectionTypeName()
                                ? CreateConstructorStatementForCollection(x, settings)
                                : $"{x.Name} = {x.Name.ToPascalCase().GetCsharpFriendlyName()};"
                        )
                );
        }
    }

    private static string CreateImmutableBuilderClassConstructorChainCall(ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.GetCustomValueForInheritedClass(settings, _ => "base()");

    private static string CreateImmutableBuilderClassCopyConstructorChainCall(ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.GetCustomValueForInheritedClass(settings, _ => "base(source)");

    private static string GenerateDefaultValueStatement(IClassProperty property, ImmutableBuilderClassSettings settings)
        => settings.UseLazyInitialization
            ? $"_{property.Name.ToPascalCase()}Delegate = new {GetNewExpression(property, settings)}(() => {property.GetDefaultValue()});"
            : $"{property.Name} = {property.GetDefaultValue()};";

    internal static string GetNewExpression(this IClassProperty property, ImmutableBuilderClassSettings settings)
        => settings.TypeSettings.UseTargetTypeNewExpressions
            ? string.Empty
            : $"System.Lazy<{CreateLazyPropertyTypeName(property, settings)}>";

    private static string CreateLazyPropertyTypeName(IClassProperty property, ImmutableBuilderClassSettings settings)
        => string.Format
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                property.TypeName,
                property.TypeName.GetGenericArguments()
            )
            .FixTypeName()
            .GetCsharpFriendlyTypeName()
            .AppendNullableAnnotation(property, settings.TypeSettings.EnableNullableReferenceTypes);

    private static IEnumerable<IClassProperty> GetImmutableBuilderConstructorProperties(ITypeBase instance,
                                                                                        ImmutableBuilderClassSettings settings,
                                                                                        bool poco)
    {
        var cls = instance as IClass;
        if (poco)
        {
            return instance.Properties;
        }

        var ctors = cls?.Constructors ?? new ReadOnlyValueCollection<IClassConstructor>();
        var ctor = ctors.FirstOrDefault(x => x.Parameters.Count > 0);
        if (ctor == null)
        {
            return instance.Properties;
        }

        if (settings.IsBuilderForOverrideEntity && settings.InheritanceSettings.BaseClass != null)
        {
            // Try to get property from either the base class c'tor or the class c'tor itself
            return ctor
                .Parameters
                .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase))
                    ?? settings.InheritanceSettings.BaseClass.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                .Where(x => x != null);
        }

        return ctor
            .Parameters
            .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
            .Where(x => x != null);
    }

    private static string GetConstructorInitializeExpressionForCollection(ImmutableBuilderClassSettings settings, IClassProperty p)
        => string.Format
        (
            p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
            p.TypeName,
            p.TypeName.GetGenericArguments()
        )
        .FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)
        .GetCsharpFriendlyTypeName();

    private static string GetCopyConstructorInitializeExpression(ImmutableBuilderClassSettings settings, IClassProperty p)
        => string.Format
        (
            p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
            p.TypeName,
            p.TypeName.GetGenericArguments()
        )
        .FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)
        .GetCsharpFriendlyTypeName();

    private static string GetImmutableBuilderClassConstructorInitializer(ImmutableBuilderClassSettings settings, IClassProperty p)
        => string.Format
        (
            p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
            p.TypeName,
            p.TypeName.GetGenericArguments()
        )
        .FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)
        .GetCsharpFriendlyTypeName();

    private static string CreateConstructorStatementForCollection(IClassProperty p, ImmutableBuilderClassSettings settings)
        => settings.ConstructorSettings.AddNullChecks
            ? $"if ({p.Name.ToPascalCase()} != null) {p.Name}.AddRange({p.Name.ToPascalCase()});"
            : $"{p.Name}.AddRange({p.Name.ToPascalCase()});";

    private static IEnumerable<ClassMethodBuilder> GetImmutableBuilderClassMethods(ITypeBase instance,
                                                                                   ImmutableBuilderClassSettings settings)
    {
        if (!(settings.InheritanceSettings.EnableEntityInheritance && settings.InheritanceSettings.IsAbstract))
        {
            var openSign = GetImmutableBuilderPocoOpenSign(instance.IsPoco());
            var closeSign = GetImmutableBuilderPocoCloseSign(instance.IsPoco());
            yield return new ClassMethodBuilder()
                .WithName("Build")
                .WithAbstract(settings.IsBuilderForAbstractEntity)
                .WithOverride(settings.IsBuilderForOverrideEntity)
                .WithTypeName(GetImmutableBuilderBuildMethodReturnType(instance, settings))
                .AddLiteralCodeStatements(settings.TypeSettings.EnableNullableReferenceTypes && !settings.IsBuilderForAbstractEntity
                    ? new[] { "#pragma warning disable CS8604 // Possible null reference argument." }
                    : Array.Empty<string>())
                .AddLiteralCodeStatements(!settings.IsBuilderForAbstractEntity
                    ? new[] { $"return new {FormatInstanceName(instance, true, settings.TypeSettings.FormatInstanceTypeNameDelegate)}{openSign}{GetConstructionMethodParameters(instance, settings)}{closeSign};" }
                    : Array.Empty<string>())
                .AddLiteralCodeStatements(settings.TypeSettings.EnableNullableReferenceTypes && !settings.IsBuilderForAbstractEntity
                    ? new[] { "#pragma warning restore CS8604 // Possible null reference argument." }
                    : Array.Empty<string>());

        }

        foreach (var classMethodBuilder in GetImmutableBuilderClassPropertyMethods(instance, settings, false))
        {
            yield return classMethodBuilder;
        }
    }

    private static string GetImmutableBuilderBuildMethodReturnType(ITypeBase instance, ImmutableBuilderClassSettings settings)
        => settings.IsBuilderForAbstractEntity
            ? "TEntity"
            : FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate);

    private static IEnumerable<ClassMethodBuilder> GetImmutableBuilderClassPropertyMethods(ITypeBase instance,
                                                                                           ImmutableBuilderClassSettings settings,
                                                                                           bool extensionMethod)
    {
        if (string.IsNullOrEmpty(settings.NameSettings.SetMethodNameFormatString)
            && string.IsNullOrEmpty(settings.NameSettings.AddMethodNameFormatString))
        {
            yield break;
        }

        foreach (var property in GetPropertiesFromClassAndBaseClass(instance, settings))
        {
            var overloads = GetOverloads(property);
            if (ShouldCreateCollectionProperty(settings, property))
            {
                yield return CreateCollectionPropertyWithEnumerableParameter(instance, settings, extensionMethod, property);
                yield return CreateCollectionPropertyWithArrayParameter(instance, settings, extensionMethod, property);

                foreach (var overload in overloads)
                {
                    yield return CreateCollectionPropertyOverloadWithEnumerableParameter(instance, settings, extensionMethod, property, overload);
                    yield return CreateCollectionPropertyOverloadWithArrayParameter(instance, settings, extensionMethod, property, overload);
                }
            }
            else if (ShouldCreateSingleProperty(settings))
            {
                yield return CreateSingleProperty(instance, settings, extensionMethod, false, property);
                if (settings.UseLazyInitialization)
                {
                    yield return CreateSingleProperty(instance, settings, extensionMethod, true, property);
                }
                foreach (var overload in overloads)
                {
                    yield return CreateSinglePropertyOverload(instance, settings, extensionMethod, property, overload);
                }
            }
        }
    }

    private static IEnumerable<IClassProperty> GetPropertiesFromClassAndBaseClass(ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.Properties
            .Concat(settings.InheritanceSettings.BaseClass?.Properties.Select(x => x.EnsureParentTypeFullName(settings.InheritanceSettings.BaseClass!))
                ?? Enumerable.Empty<IClassProperty>())
            .Where(x => instance.IsMemberValidForImmutableBuilderClass(x,
                                                                       settings.InheritanceSettings,
                                                                       isForWithStatement: true));

    private static bool ShouldCreateSingleProperty(ImmutableBuilderClassSettings settings)
        => !string.IsNullOrEmpty(settings.NameSettings.SetMethodNameFormatString);

    private static bool ShouldCreateCollectionProperty(ImmutableBuilderClassSettings settings, IClassProperty property)
        => property.TypeName.IsCollectionTypeName()
            && !string.IsNullOrEmpty(settings.NameSettings.AddMethodNameFormatString);

    private static ClassMethodBuilder CreateCollectionPropertyOverloadWithArrayParameter(ITypeBase instance,
                                                                                         ImmutableBuilderClassSettings settings,
                                                                                         bool extensionMethod,
                                                                                         IClassProperty property,
                                                                                         Overload overload)
        => new ClassMethodBuilder()
            .WithName(string.Format(overload.MethodName.WhenNullOrEmpty(settings.NameSettings.AddMethodNameFormatString), property.Name))
            .ConfigureForExtensionMethod(instance, settings, extensionMethod)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName(property.Name.ToPascalCase())
                    .WithIsParamArray()
                    .WithTypeName(string.Format(overload.ArgumentType,
                                                property.TypeName,
                                                property.TypeName.GetGenericArguments()).ConvertTypeNameToArray())
                    .WithIsNullable(property.IsNullable)
            )
            .AddLiteralCodeStatements(GetImmutableBuilderAddOverloadMethodStatements(settings,
                                                                                     property,
                                                                                     overload.InitializeExpression,
                                                                                     extensionMethod));

    private static ClassMethodBuilder CreateCollectionPropertyOverloadWithEnumerableParameter(ITypeBase instance,
                                                                                              ImmutableBuilderClassSettings settings,
                                                                                              bool extensionMethod,
                                                                                              IClassProperty property,
                                                                                              Overload overload)
        => new ClassMethodBuilder()
            .WithName(string.Format(overload.MethodName.WhenNullOrEmpty(settings.NameSettings.AddMethodNameFormatString), property.Name))
            .ConfigureForExtensionMethod(instance, settings, extensionMethod)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName(property.Name.ToPascalCase())
                    .WithTypeName(string.Format(overload.ArgumentType,
                                                property.TypeName,
                                                property.TypeName.GetGenericArguments()).FixCollectionTypeName(typeof(IEnumerable<>).WithoutGenerics()))
                    .WithIsNullable(property.IsNullable)
            )
            .AddLiteralCodeStatements($"return {string.Format(overload.MethodName.WhenNullOrEmpty(settings.NameSettings.AddMethodNameFormatString), property.Name)}({property.Name.ToPascalCase()}.ToArray());");

    private static ClassMethodBuilder CreateCollectionPropertyWithEnumerableParameter(ITypeBase instance,
                                                                                      ImmutableBuilderClassSettings settings,
                                                                                      bool extensionMethod,
                                                                                      IClassProperty property)
        => new ClassMethodBuilder()
            .WithName(string.Format(settings.NameSettings.AddMethodNameFormatString, property.Name))
            .ConfigureForExtensionMethod(instance, settings, extensionMethod)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName(property.Name.ToPascalCase())
                    .WithTypeName
                    (
                        string.Format
                        (
                            property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                            property.TypeName,
                            property.TypeName.GetGenericArguments()
                        ).FixCollectionTypeName(typeof(IEnumerable<>).WithoutGenerics())
                    )
                    .WithIsNullable(property.IsNullable)
            )
            .AddLiteralCodeStatements($"return {GetCallPrefix(extensionMethod, false)}Add{property.Name}({property.Name.ToPascalCase()}.ToArray());");

    private static ClassMethodBuilder CreateCollectionPropertyWithArrayParameter(ITypeBase instance,
                                                                                 ImmutableBuilderClassSettings settings,
                                                                                 bool extensionMethod,
                                                                                 IClassProperty property)
        => new ClassMethodBuilder()
            .WithName(string.Format(settings.NameSettings.AddMethodNameFormatString, property.Name))
            .ConfigureForExtensionMethod(instance, settings, extensionMethod)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName(property.Name.ToPascalCase())
                    .WithIsParamArray()
                    .WithTypeName
                    (
                        string.Format
                        (
                            property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                            property.TypeName,
                            property.TypeName.GetGenericArguments()
                        )
                        .FixTypeName()
                        .ConvertTypeNameToArray()
                    ).WithIsNullable(property.IsNullable)
            ).AddLiteralCodeStatements(GetImmutableBuilderAddMethodStatements(settings, property, extensionMethod));

    private static ClassMethodBuilder CreateSingleProperty(ITypeBase instance,
                                                           ImmutableBuilderClassSettings settings,
                                                           bool extensionMethod,
                                                           bool useLazyInitialization,
                                                           IClassProperty property)
    {
        var typeName = string.Format
        (
            property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
            property.TypeName,
            property.TypeName.GetGenericArguments()
        );
        return new ClassMethodBuilder()
            .WithName(string.Format(settings.NameSettings.SetMethodNameFormatString, property.Name))
            .ConfigureForExtensionMethod(instance, settings, extensionMethod)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName(useLazyInitialization
                        ? property.Name.ToPascalCase() + "Delegate"
                        : property.Name.ToPascalCase())
                    .WithTypeName
                    (
                        useLazyInitialization
                            ? $"System.Func<{typeName.FixTypeName().AppendNullableAnnotation(property, settings.TypeSettings.EnableNullableReferenceTypes)}>"
                            : typeName
                    )
                    .WithIsNullable(!useLazyInitialization && property.IsNullable)
                    .WithDefaultValue(useLazyInitialization
                        ? null
                        : property.Metadata.GetValue<object?>(MetadataNames.CustomBuilderWithDefaultPropertyValue, () => null))
            )
            .AddLiteralCodeStatements
            (
                string.Format
                (
                    property.Metadata.GetStringValue(MetadataNames.CustomBuilderWithExpression, $"{GetCallPrefix(extensionMethod, useLazyInitialization)}{GetCallPropertyName(property.Name, useLazyInitialization)}{GetCallSuffix(useLazyInitialization)} = {GetExpressionPrefix(property, settings, useLazyInitialization)}{property.Name.ToPascalCase().GetCsharpFriendlyName()}{GetExpressionSuffix(useLazyInitialization)};"),
                    property.Name,
                    property.Name.ToPascalCase(),
                    useLazyInitialization
                        ? $"{property.Name.ToPascalCase().GetCsharpFriendlyName()}Delegate.Invoke()"
                        : property.Name.ToPascalCase().GetCsharpFriendlyName(),
                    property.TypeName,
                    property.TypeName.GetGenericArguments(),
                    "{",
                    "}"
                ),
                $"return {GetReturnValue(settings, extensionMethod)};"
            );
    }

    private static ClassMethodBuilder CreateSinglePropertyOverload(ITypeBase instance,
                                                                   ImmutableBuilderClassSettings settings,
                                                                   bool extensionMethod,
                                                                   IClassProperty property,
                                                                   Overload overload)
        => new ClassMethodBuilder()
            .WithName(string.Format(overload.MethodName.WhenNullOrEmpty(settings.NameSettings.SetMethodNameFormatString),
                                    property.Name))
            .ConfigureForExtensionMethod(instance, settings, extensionMethod)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName(overload.ArgumentName.WhenNullOrEmpty(() => property.Name.ToPascalCase()))
                    .WithTypeName(string.Format(overload.ArgumentType, property.TypeName))
                    .WithIsNullable(property.IsNullable)
            )
            .AddLiteralCodeStatements
            (
                string.Format(overload.InitializeExpression,
                              property.Name.ToPascalCase(),
                              property.TypeName.FixTypeName().GetCsharpFriendlyTypeName(),
                              property.Name),
                $"return {GetReturnValue(settings, extensionMethod)};"
            );

    private static ClassMethodBuilder ConfigureForExtensionMethod(this ClassMethodBuilder builder,
                                                                  ITypeBase instance,
                                                                  ImmutableBuilderClassSettings settings,
                                                                  bool extensionMethod)
        => builder.WithTypeName(settings.IsBuilderForAbstractEntity ? "TBuilder" : $"{instance.Name}Builder")
                  .WithStatic(extensionMethod)
                  .WithExtensionMethod(extensionMethod)
                  .AddParameters(new[]
                  {
                              new ParameterBuilder().WithName("instance")
                                                    .WithTypeName($"{instance.Name}Builder")
                  }.Where(_ => extensionMethod));

    private static string GetCallPrefix(bool extensionMethod, bool lazyInitialization)
        => extensionMethod
            ? $"instance.{GetLazyInitializationPrefix(lazyInitialization)}"
            : $"{GetLazyInitializationPrefix(lazyInitialization)}";

    private static string GetLazyInitializationPrefix(bool lazyInitialization)
        => lazyInitialization
            ? "_"
            : string.Empty;

    private static string GetCallPropertyName(string name, bool lazyInitialization)
        => lazyInitialization
            ? name.ToPascalCase()
            : name;

    private static string GetCallSuffix(bool lazyInitialization)
        => lazyInitialization
            ? "Delegate"
            : string.Empty;

    private static string GetExpressionPrefix(IClassProperty property, ImmutableBuilderClassSettings settings, bool useLazyInitialization)
        => useLazyInitialization
            ? $"new {property.GetNewExpression(settings)}("
            : string.Empty;

    private static string GetExpressionSuffix(bool useLazyInitialization)
        => useLazyInitialization
            ? "Delegate)"
            : string.Empty;

    private static IEnumerable<Overload> GetOverloads(IClassProperty property)
    {
        var argumentTypes = property.Metadata.GetStringValues(MetadataNames.CustomBuilderWithOverloadArgumentType).ToArray();
        var initializeExpressions = property.Metadata.GetStringValues(MetadataNames.CustomBuilderWithOverloadInitializeExpression).ToArray();
        var methodNames = property.Metadata.GetStringValues(MetadataNames.CustomBuilderWithOverloadMethodName).ToArray();
        var argumentNames = property.Metadata.GetStringValues(MetadataNames.CustomBuilderWithOverloadArgumentName).ToArray();

        if (argumentTypes.Length != initializeExpressions.Length
            || argumentTypes.Length != methodNames.Length
            || argumentTypes.Length != argumentNames.Length)
        {
            throw new InvalidOperationException("Metadata for immutable builder overload method is incorrect. Metadata needs to be available in the same amount for all metadata types");
        }

        return
            from argumentType in argumentTypes
            from initializeExpression in initializeExpressions
            from methodName in methodNames
            from argumentName in argumentNames
            select new Overload(argumentType, initializeExpression, methodName, argumentName);
    }

    private static List<string> GetImmutableBuilderAddMethodStatements(ImmutableBuilderClassSettings settings,
                                                                       IClassProperty property,
                                                                       bool extensionMethod)
        => settings.ConstructorSettings.AddNullChecks
            ? new[]
                {
                    $"if ({property.Name.ToPascalCase()} != null)",
                    "{",
                    string.Format
                    (
                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderAddExpression, $"    {GetCallPrefix(extensionMethod, false)}{property.Name}.AddRange({property.Name.ToPascalCase()});"),
                        property.Name.ToPascalCase(),
                        property.TypeName,
                        property.TypeName.GetGenericArguments()
                    ),
                    "}",
                    $"return {GetReturnValue(settings, extensionMethod)};"
                }.ToList()
            : new[]
                {
                    string.Format
                    (
                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderAddExpression, $"{GetCallPrefix(extensionMethod, false)}{property.Name}.AddRange({property.Name.ToPascalCase()});"),
                        property.Name.ToPascalCase(),
                        property.TypeName,
                        property.TypeName.GetGenericArguments()
                    ),
                    $"return {GetReturnValue(settings, extensionMethod)};"
                }.ToList();

    private static string GetReturnValue(ImmutableBuilderClassSettings settings, bool extensionMethod)
    {
        if (extensionMethod)
        {
            return "instance";
        }

        if (settings.IsBuilderForAbstractEntity)
        {
            return "(TBuilder)this";
        }

        return "this";
    }

    private static List<string> GetImmutableBuilderAddOverloadMethodStatements(ImmutableBuilderClassSettings settings,
                                                                               IClassProperty property,
                                                                               string overloadExpression,
                                                                               bool extensionMethod)
        => settings.ConstructorSettings.AddNullChecks
            ? (new[]
            {
                $"if ({property.Name.ToPascalCase()} != null)",
                    "{",
                    string.Format(overloadExpression,
                                  property.Name.ToPascalCase(),
                                  property.TypeName.FixTypeName(),
                                  property.TypeName.GetGenericArguments(),
                                  CreateIndentForImmutableBuilderAddOverloadMethodStatement(settings),
                                  property.Name),
                    "    }",
                    "}",
                    $"return {GetReturnValue(settings, extensionMethod)};"
            }).ToList()
            : (new[]
            {
                string.Format(overloadExpression,
                              property.Name.ToPascalCase(),
                              property.TypeName.FixTypeName(),
                              property.TypeName.GetGenericArguments(),
                              CreateIndentForImmutableBuilderAddOverloadMethodStatement(settings),
                              property.Name),
                $"return {GetReturnValue(settings, extensionMethod)};"
            }).ToList();

    private static string CreateIndentForImmutableBuilderAddOverloadMethodStatement(ImmutableBuilderClassSettings settings)
        => settings.ConstructorSettings.AddNullChecks
            ? "        "
            : "    ";

    private static string GetImmutableBuilderPocoCloseSign(bool poco)
        => poco
            ? " }"
            : ")";

    private static string GetImmutableBuilderPocoOpenSign(bool poco)
        => poco
            ? " { "
            : "(";

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

    private static IEnumerable<ClassPropertyBuilder> GetImmutableBuilderClassProperties(ITypeBase instance,
                                                                                        ImmutableBuilderClassSettings settings)
        => instance.Properties
            .Where
            (
                x => !settings.IsAbstractBuilder && instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement: false)
            )
            .Select(property => new ClassPropertyBuilder()
                .WithName(property.Name)
                .WithTypeName
                (
                    string.Format
                    (
                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                        property.TypeName,
                        property.TypeName.GetGenericArguments()
                    ).FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)
                )
                .WithIsNullable(property.IsNullable)
                .AddAttributes(property.Attributes.Select(x => new AttributeBuilder(x)))
                .AddMetadata(property.Metadata.Select(x => new MetadataBuilder(x)))
                .AddGetterCodeStatements(CreateImmutableBuilderPropertyGetterStatements(property, settings))
                .AddSetterCodeStatements(CreateImmutableBuilderPropertySetterStatements(property, settings))
            );

    private static IEnumerable<ICodeStatementBuilder> CreateImmutableBuilderPropertyGetterStatements(IClassProperty property,
                                                                                                     ImmutableBuilderClassSettings settings)
    {
        if (settings.UseLazyInitialization && !property.TypeName.IsCollectionTypeName())
        {
            yield return new LiteralCodeStatementBuilder($"return _{property.Name.ToPascalCase()}Delegate.Value;");
        }
        else if (settings.CopyPropertyCode)
        {
            foreach (var statement in property.GetterCodeStatements.Select(x => x.CreateBuilder()))
            {
                yield return statement;
            }
        }
    }

    private static IEnumerable<ICodeStatementBuilder> CreateImmutableBuilderPropertySetterStatements(IClassProperty property,
                                                                                                     ImmutableBuilderClassSettings settings)
    {
        if (settings.UseLazyInitialization && !property.TypeName.IsCollectionTypeName())
        {
            yield return new LiteralCodeStatementBuilder($"_{property.Name.ToPascalCase()}Delegate = new {GetNewExpression(property, settings)}(() => value);");
        }
        else if (settings.CopyPropertyCode)
        {
            foreach (var statement in property.SetterCodeStatements.Select(x => x.CreateBuilder()))
            {
                yield return statement;
            }
        }
    }

    private static string GetConstructionMethodParameters(ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        var poco = instance.IsPoco();
        var properties = GetImmutableBuilderConstructorProperties(instance, settings, poco);

        var defaultValueDelegate = poco
            ? new Func<IClassProperty, string>(p => $"{p.Name} = {p.Name}")
            : new Func<IClassProperty, string>(p => $"{p.Name}");

        return string.Join
        (
            ", ",
            properties.Select(p => string.Format(p.Metadata.GetStringValue(MetadataNames.CustomBuilderMethodParameterExpression, defaultValueDelegate(p)),
                                                 p.Name,
                                                 p.Name.ToPascalCase(),
                                                 p.IsNullable ? "?" : ""))
        );
    }
}
