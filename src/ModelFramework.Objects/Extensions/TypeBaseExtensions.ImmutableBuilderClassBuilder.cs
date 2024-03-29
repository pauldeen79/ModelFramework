﻿namespace ModelFramework.Objects.Extensions;

public static partial class TypeBaseExtensions
{
    public static IClass ToImmutableBuilderClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.ToImmutableBuilderClassBuilder(settings).BuildTyped();

    public static ClassBuilder ToImmutableBuilderClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        if (!settings.GenerationSettings.AllowGenerationWithoutProperties && !instance.Properties.Any() && !settings.InheritanceSettings.EnableEntityInheritance)
        {
            throw new InvalidOperationException("To create an immutable builder class, there must be at least one property");
        }

        return new ClassBuilder()
            .WithName(string.Format(settings.NameSettings.BuilderNameFormatString, instance.Name))
            .AddGenericTypeArguments(new[] { "TBuilder", "TEntity" }.Where(_ => settings.IsBuilderForAbstractEntity))
            .AddGenericTypeArgumentConstraints(new[] { $"where TEntity : {FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate)}" }.Where(_ => settings.IsBuilderForAbstractEntity))
            .AddGenericTypeArgumentConstraints(new[] { $"where TBuilder : {string.Format(settings.NameSettings.BuilderNameFormatString, instance.Name)}<TBuilder, TEntity>" }.Where(_ => settings.IsBuilderForAbstractEntity))
            .WithNamespace(settings.NameSettings.BuildersNamespace.WhenNullOrEmpty(instance.Namespace))
            .WithBaseClass(GetImmutableBuilderClassBaseClass(instance, settings))
            .WithAbstract(settings.IsBuilderForAbstractEntity)
            .WithPartial()
            .AddConstructors(GetImmutableBuilderClassConstructors(instance, settings))
            .AddMethods(GetImmutableBuilderClassMethods(instance, settings))
            .AddProperties(GetImmutableBuilderClassProperties(instance, settings))
            .AddAttributes(instance.Attributes.Where(_ => settings.GenerationSettings.CopyAttributes).Select(x => new AttributeBuilder(x)))
            .AddFields(GetImmutableBuilderClassFields(instance, settings, false))
            .AddGenericTypeArguments(instance.GenericTypeArguments)
            .AddGenericTypeArgumentConstraints(instance.GenericTypeArgumentConstraints)
            .AddInterfaces(new[] { typeof(IValidatableObject) }.Where(_ => settings.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared && !settings.IsBuilderForAbstractEntity));
    }

    public static IClass ToBuilderExtensionsClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.ToBuilderExtensionsClassBuilder(settings).BuildTyped();

    public static ClassBuilder ToBuilderExtensionsClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        if (!settings.GenerationSettings.AllowGenerationWithoutProperties && !instance.Properties.Any() && !settings.InheritanceSettings.EnableEntityInheritance)
        {
            throw new InvalidOperationException("To create a builder extensions class, there must be at least one property");
        }

        return new ClassBuilder()
            .WithName(instance.Name + "BuilderExtensions")
            .WithNamespace(settings.NameSettings.BuildersNamespace.WhenNullOrEmpty(instance.Namespace))
            .WithPartial()
            .WithStatic()
            .AddMethods(GetImmutableBuilderClassPropertyMethods(instance, settings, true));
    }

    public static IClass ToNonGenericImmutableBuilderClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.ToNonGenericImmutableBuilderClassBuilder(settings).BuildTyped();

    public static ClassBuilder ToNonGenericImmutableBuilderClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        settings = settings.ForAbstractBuilder();

        return new ClassBuilder()
            .WithName(string.Format(settings.NameSettings.BuilderNameFormatString, instance.Name))
            .WithNamespace(settings.NameSettings.BuildersNamespace.WhenNullOrEmpty(instance.Namespace))
            .WithBaseClass(GetImmutableBuilderClassBaseClass(instance, settings))
            .WithAbstract(settings.IsBuilderForAbstractEntity)
            .WithPartial()
            .AddConstructors(GetImmutableBuilderClassConstructors(instance, settings))
            .AddProperties(GetImmutableBuilderClassProperties(instance, settings))
            .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)))
            .AddFields(GetImmutableBuilderClassFields(instance, settings, false))
            .AddMethods
            (
                new ClassMethodBuilder()
                    .WithName(settings.NameSettings.BuildMethodName)
                    .WithAbstract()
                    .WithTypeName(FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate))
            )
            .AddGenericTypeArguments(instance.GenericTypeArguments)
            .AddGenericTypeArgumentConstraints(instance.GenericTypeArgumentConstraints);
    }

    private static string GetImmutableBuilderClassBaseClass(ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        var genericTypeArgumentsString = instance.GetGenericTypeArgumentsString();

        if (settings.InheritanceSettings.EnableEntityInheritance
            && settings.InheritanceSettings.EnableBuilderInheritance
            && settings.InheritanceSettings.BaseClass == null
            && !settings.IsForAbstractBuilder)
        {
            return string.Format(settings.NameSettings.BuilderNameFormatString, instance.Name) + genericTypeArgumentsString;
        }

        if (settings.InheritanceSettings.EnableEntityInheritance
            && settings.InheritanceSettings.EnableBuilderInheritance
            && settings.InheritanceSettings.BaseClass != null
            && !settings.IsForAbstractBuilder
            && settings.InheritanceSettings.IsAbstract)
        {
            return string.Format(settings.NameSettings.BuilderNameFormatString, instance.Name) + genericTypeArgumentsString;
        }

        if (settings.InheritanceSettings.EnableEntityInheritance
            && settings.InheritanceSettings.EnableBuilderInheritance
            && settings.InheritanceSettings.BaseClass != null
            && !settings.IsForAbstractBuilder
            && settings.InheritanceSettings.RemoveDuplicateWithMethods)
        {
            var ns = string.IsNullOrEmpty(settings.InheritanceSettings.BaseClassBuilderNameSpace)
                ? string.Empty
                : $"{settings.InheritanceSettings.BaseClassBuilderNameSpace}.";
            return $"{ns}{string.Format(settings.NameSettings.BuilderNameFormatString, settings.InheritanceSettings.BaseClass.Name)}<{string.Format(settings.NameSettings.BuilderNameFormatString, instance.Name)}{genericTypeArgumentsString}, {FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate)}{genericTypeArgumentsString}>";
        }

        var builderName = string.Format(settings.NameSettings.BuilderNameFormatString, instance.Name);

        return instance.GetCustomValueForInheritedClass(settings, cls => settings.InheritanceSettings.EnableBuilderInheritance && settings.InheritanceSettings.BaseClass != null
            ? $"{string.Format(settings.NameSettings.BuilderNameFormatString, cls.BaseClass.GetClassName())}{genericTypeArgumentsString}"
            : $"{string.Format(settings.NameSettings.BuilderNameFormatString, cls.BaseClass.GetClassName())}<{builderName}{genericTypeArgumentsString}, {FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate)}{genericTypeArgumentsString}>");
    }

    private static IEnumerable<ClassFieldBuilder> GetImmutableBuilderClassFields(ITypeBase instance, ImmutableBuilderClassSettings settings, bool isForWithStatement)
    {
        if (settings.IsAbstractBuilder)
        {
            yield break;
        }

        if (settings.GenerationSettings.CopyFields)
        {
            foreach (var field in instance.GetFields()
            .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement))
            .Select(x => new ClassFieldBuilder(x).WithProtected()))
            {
                yield return field;
            }
        }

        if (settings.GenerationSettings.UseLazyInitialization)
        {
            foreach (var property in instance.Properties
                .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement))
                .Where(x => !x.TypeName.IsCollectionTypeName()))
            {
                yield return new ClassFieldBuilder()
                    .WithName($"_{property.Name.ToPascalCase()}Delegate")
                    .WithTypeName($"{typeof(Lazy<>).WithoutGenerics()}<{CreatePropertyTypeName(property, settings)}>")
                    .WithProtected();
            }
        }

        if (settings.ConstructorSettings.AddNullChecks && settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            foreach (var property in instance.Properties
                .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement)))
            {
                yield return new ClassFieldBuilder()
                    .WithName($"_{property.Name.ToPascalCase()}")
                    .WithTypeName
                    (
                        string.Format
                        (
                            property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                            property.TypeName,                                     // 0
                            property.TypeName.GetGenericArguments(),               // 1
                            property.TypeName.GetClassName(),                      // 2
                            property.TypeName.GetGenericArguments().GetClassName() // 3
                        ).FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)
                    )
                    .WithIsNullable(property.IsNullable)
                    .WithIsValueType(property.IsValueType);
            }
        }
    }

    private static IEnumerable<ClassConstructorBuilder> GetImmutableBuilderClassConstructors(ITypeBase instance,
                                                                                             ImmutableBuilderClassSettings settings)
    {
        if (!settings.NeedsConstructors)
        {
            yield break;
        }

        if (settings.InheritanceSettings.EnableBuilderInheritance
            && settings.IsAbstractBuilder
            && !settings.IsForAbstractBuilder)
        {
            yield return new ClassConstructorBuilder()
                .WithChainCall("base()")
                .WithProtected(settings.IsBuilderForAbstractEntity);

            if (settings.ConstructorSettings.AddCopyConstructor)
            {
                yield return new ClassConstructorBuilder()
                    .WithChainCall("base(source)")
                    .WithProtected(settings.IsBuilderForAbstractEntity)
                    .AddParameters
                    (
                        new ParameterBuilder()
                            .WithName("source")
                            .WithTypeName(FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate) + instance.GetGenericTypeArgumentsString())
                    )
                    .AddParameters
                    (
                        instance.Metadata
                            .GetValues<IParameter>(MetadataNames.AdditionalBuilderCopyConstructorAdditionalParameter)
                            .Select(x => new ParameterBuilder(x))
                    );
            }

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
                    .Select(x => $"{GetInitializationName(x.Name, settings)} = {GetImmutableBuilderClassConstructorInitializer(settings, x)};")
            )
            .AddLiteralCodeStatements
            (
                instance.Properties
                    .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement: false))
                    .Where(x => settings.ConstructorSettings.SetDefaultValues
                        && !x.TypeName.IsCollectionTypeName()
                        && (!x.IsNullable || settings.GenerationSettings.UseLazyInitialization)
                        && (!IsNoNullableDefault(settings, x) || settings.GenerationSettings.UseLazyInitialization))
                    .Select(x => GenerateDefaultValueStatement(x, settings))
            );

        if (settings.ConstructorSettings.AddCopyConstructor)
        {
            yield return CreateCopyConstructor(instance, settings);
        }

        if (settings.ConstructorSettings.AddConstructorWithAllProperties)
        {

            yield return CreateConstructorWithAllProperties(instance, settings);
        }
    }

    private static bool IsNoNullableDefault(ImmutableBuilderClassSettings settings, IClassProperty x)
    {
        var value = x.GetDefaultValue(settings.TypeSettings.EnableNullableReferenceTypes);
        return value.StartsWith("default(", StringComparison.Ordinal) && !value.EndsWith("!");
    }

    private static ClassConstructorBuilder CreateConstructorWithAllProperties(ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        var properties = GetImmutableBuilderConstructorProperties(instance, settings, true);
        return new ClassConstructorBuilder()
            .AddParameters
            (
                properties.Select(p => CreateParameterForConstructorWithAllProperties(settings, p))
            )
            .AddLiteralCodeStatements
            (
                properties
                    .Where(p => p.TypeName.IsCollectionTypeName())
                    .Select(p => $"{p.Name} = {GetConstructorInitializeExpressionForCollection(settings, p)};")
            )
            .AddLiteralCodeStatements
            (
                instance.Properties
                    .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement: false))
                    .Where(x => !settings.GenerationSettings.UseLazyInitialization || x.TypeName.IsCollectionTypeName())
                    .Select
                    (
                        x => x.TypeName.IsCollectionTypeName()
                            ? CreateConstructorStatementForCollection(x, settings)
                            : $"{GetInitializationName(x.Name, settings)} = {x.Name.ToPascalCase().GetCsharpFriendlyName()};"
                    )
            );
    }

    private static ParameterBuilder CreateParameterForConstructorWithAllProperties(ImmutableBuilderClassSettings settings, IClassProperty p)
        => new ParameterBuilder()
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
            .WithIsValueType(p.IsValueType);

    private static ClassConstructorBuilder CreateCopyConstructor(ITypeBase instance, ImmutableBuilderClassSettings settings)
        => new ClassConstructorBuilder()
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
                        $@"    throw new {typeof(ArgumentNullException).FullName}(""source"");",
                        "}"
                    );
                }
            })
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName(FormatInstanceName(instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate) + instance.GetGenericTypeArgumentsString())
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
                    .Select(x => $"{GetInitializationName(x.Name, settings)} = {GetCopyConstructorInitializeExpression(settings, x)};")
            )
            .AddLiteralCodeStatements
            (
                instance.Properties
                    .Where(x => instance.IsMemberValidForImmutableBuilderClass(x, settings.InheritanceSettings, isForWithStatement: false))
                    .Select(x => $"{x.CreateImmutableBuilderInitializationCode(settings)};")
            );

    private static string CreateImmutableBuilderClassConstructorChainCall(ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.GetCustomValueForInheritedClass(settings, _ => "base()");

    private static string CreateImmutableBuilderClassCopyConstructorChainCall(ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.GetCustomValueForInheritedClass(settings, _ => "base(source)");

    private static string GenerateDefaultValueStatement(IClassProperty property, ImmutableBuilderClassSettings settings)
    {
        if (settings.GenerationSettings.UseLazyInitialization)
        {
            return $"_{property.Name.ToPascalCase()}Delegate = new {GetNewExpression(property, settings)}(() => {GetNewBuilderExpression(property, settings)});";
        }

        return $"{GetInitializationName(property.Name, settings)} = {property.GetDefaultValue(settings.TypeSettings.EnableNullableReferenceTypes)};";
    }

    private static string GetInitializationName(string name, ImmutableBuilderClassSettings settings)
    {
        if (settings.ConstructorSettings.AddNullChecks && settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            return $"_{name.ToPascalCase()}";
        }

        return name;
    }

    private static string GetNewBuilderExpression(IClassProperty property, ImmutableBuilderClassSettings settings)
    {
        var md = property.Metadata.FirstOrDefault(x => x.Name == MetadataNames.CustomBuilderDefaultValue);
        if (md != null && md.Value != null)
        {
            if (md.Value is Literal literal && literal.Value != null)
            {
                return literal.Value;
            }
            return md.Value.CsharpFormat();
        }

        return CreatePropertyTypeName(property, settings)
            .GetDefaultValue(property.IsNullable, property.IsValueType, settings.TypeSettings.EnableNullableReferenceTypes);
    }

    internal static string GetNewExpression(this IClassProperty property, ImmutableBuilderClassSettings settings)
        => settings.TypeSettings.UseTargetTypeNewExpressions
            ? string.Empty
            : $"{typeof(Lazy<>).WithoutGenerics()}<{CreatePropertyTypeName(property, settings)}>";

    private static string CreatePropertyTypeName(IClassProperty property, ImmutableBuilderClassSettings settings)
        => string.Format
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                property.TypeName,                                     // 0
                property.TypeName.GetGenericArguments(),               // 1
                property.TypeName.GetClassName(),                      // 2
                property.TypeName.GetGenericArguments().GetClassName() // 3
            )
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
            p.TypeName,                                     // 0
            p.TypeName.GetGenericArguments(),               // 1
            p.TypeName.GetClassName(),                      // 2
            p.TypeName.GetGenericArguments().GetClassName() // 3
        )
        .FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)
        .GetCollectionInitializeStatement()
        .GetCsharpFriendlyTypeName();

    private static string GetCopyConstructorInitializeExpression(ImmutableBuilderClassSettings settings, IClassProperty p)
        => string.Format
        (
            p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
            p.TypeName,                                     // 0
            p.TypeName.GetGenericArguments(),               // 1
            p.TypeName.GetClassName(),                      // 2
            p.TypeName.GetGenericArguments().GetClassName() // 3
        )
        .FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)
        .GetCollectionInitializeStatement()
        .GetCsharpFriendlyTypeName();

    private static string GetImmutableBuilderClassConstructorInitializer(ImmutableBuilderClassSettings settings, IClassProperty p)
        => string.Format
        (
            p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
            p.TypeName,                                     // 0
            p.TypeName.GetGenericArguments(),               // 1
            p.TypeName.GetClassName(),                      // 2
            p.TypeName.GetGenericArguments().GetClassName() // 3
        )
        .FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)
        .GetCollectionInitializeStatement()
        .GetCsharpFriendlyTypeName();

    private static string CreateConstructorStatementForCollection(IClassProperty property, ImmutableBuilderClassSettings settings)
        => settings.ConstructorSettings.AddNullChecks
            ? $"if ({property.Name.ToPascalCase().GetCsharpFriendlyName()} != null) {property.Name}.AddRange({property.Name.ToPascalCase()});"
            : $"{property.Name}.AddRange({property.Name.ToPascalCase()});";

    private static IEnumerable<ClassMethodBuilder> GetImmutableBuilderClassMethods(ITypeBase instance,
                                                                                   ImmutableBuilderClassSettings settings)
    {
        if (!(settings.InheritanceSettings.EnableBuilderInheritance && settings.InheritanceSettings.IsAbstract))
        {
            yield return FillBuildMethod(instance, new ClassMethodBuilder()
                .WithName(settings.IsBuilderForAbstractEntity || settings.IsBuilderForOverrideEntity
                    ? settings.NameSettings.BuildTypedMethodName
                    : settings.NameSettings.BuildMethodName)
                .WithAbstract(settings.IsBuilderForAbstractEntity)
                .WithOverride(settings.IsBuilderForOverrideEntity)
                .WithTypeName(GetImmutableBuilderBuildMethodReturnType(instance, settings) + instance.GetGenericTypeArgumentsString()), settings);

            if (settings.IsBuilderForAbstractEntity)
            {
                yield return new ClassMethodBuilder()
                    .WithName(settings.NameSettings.BuildMethodName)
                    .WithOverride()
                    .WithTypeName(FormatInstanceName(settings.InheritanceSettings.BaseClass ?? instance, false, settings.TypeSettings.FormatInstanceTypeNameDelegate) + (settings.InheritanceSettings.BaseClass ?? instance).GetGenericTypeArgumentsString())
                    .AddLiteralCodeStatements($"return {settings.NameSettings.BuildTypedMethodName}();");
            }
        }

        if (settings.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared && !settings.IsBuilderForAbstractEntity)
        {
            // Allow validation of the builder by calling the validate method on the entity
            yield return CreateValidateMethod(instance, settings);
        }

        foreach (var classMethodBuilder in GetImmutableBuilderClassPropertyMethods(instance, settings, false))
        {
            yield return classMethodBuilder;
        }
    }

    private static ClassMethodBuilder CreateValidateMethod(ITypeBase instance, ImmutableBuilderClassSettings settings)
        => new ClassMethodBuilder()
            .WithName(nameof(IValidatableObject.Validate))
            .WithType(typeof(IEnumerable<ValidationResult>))
            .AddParameter("validationContext", typeof(ValidationContext))
            .AddLiteralCodeStatements(CreatePragmaWarningDisableStatements(settings))
            .AddLiteralCodeStatements($"var instance = {CreateEntityInstanciation(instance, settings, "Base")};")
            .AddLiteralCodeStatements(CreatePragmaWarningRestoreStatements(settings))
            .AddLiteralCodeStatements
            (
                $"var results = new {typeof(List<>).WithoutGenerics()}<{typeof(ValidationResult).FullName}>();",
                $"{typeof(Validator).FullName}.{nameof(Validator.TryValidateObject)}(instance, new {typeof(ValidationContext).FullName}(instance), results, true);",
                "return results;"
            );

    private static string[] CreatePragmaWarningDisableStatements(ImmutableBuilderClassSettings settings)
        => settings.TypeSettings.EnableNullableReferenceTypes && !settings.IsBuilderForAbstractEntity && !settings.ConstructorSettings.AddNullChecks
            ?
            [
                "#pragma warning disable CS8604 // Possible null reference argument.",
                "#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.",
            ]
            : Array.Empty<string>();

    private static string[] CreatePragmaWarningRestoreStatements(ImmutableBuilderClassSettings settings)
        => settings.TypeSettings.EnableNullableReferenceTypes && !settings.IsBuilderForAbstractEntity && !settings.ConstructorSettings.AddNullChecks
            ?
            [
                "#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.",
                "#pragma warning restore CS8604 // Possible null reference argument.",
            ]
            : Array.Empty<string>();

    private static ClassMethodBuilder FillBuildMethod(
        ITypeBase instance,
        ClassMethodBuilder classMethodBuilder,
        ImmutableBuilderClassSettings settings)
        => classMethodBuilder
            .AddLiteralCodeStatements(CreatePragmaWarningDisableStatements(settings))
            .AddLiteralCodeStatements
            (
                !settings.IsBuilderForAbstractEntity
                    ? [$"return {CreateEntityInstanciation(instance, settings, string.Empty)};"]
                    : Array.Empty<string>()
            )
            .AddLiteralCodeStatements(CreatePragmaWarningRestoreStatements(settings));

    private static string CreateEntityInstanciation(
        ITypeBase instance,
        ImmutableBuilderClassSettings settings,
        string classNameSuffix)
    {
        var openSign = GetImmutableBuilderPocoOpenSign(instance.IsPoco());
        var closeSign = GetImmutableBuilderPocoCloseSign(instance.IsPoco());
        return $"new {FormatInstanceName(instance, true, settings.TypeSettings.FormatInstanceTypeNameDelegate)}{classNameSuffix}{instance.GetGenericTypeArgumentsString()}{openSign}{GetConstructionMethodParameters(instance, settings)}{closeSign}";
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
                    yield return CreateCollectionPropertyOverload(instance, settings, extensionMethod, property, overload);
                }
            }
            else if (ShouldCreateSingleProperty(settings, property))
            {
                yield return CreateSingleProperty(instance, settings, extensionMethod, false, property);
                if (settings.GenerationSettings.UseLazyInitialization)
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

    private static bool ShouldCreateSingleProperty(ImmutableBuilderClassSettings settings, IClassProperty property)
        => !string.IsNullOrEmpty(settings.NameSettings.SetMethodNameFormatString) && !property.TypeName.IsCollectionTypeName();

    private static bool ShouldCreateCollectionProperty(ImmutableBuilderClassSettings settings, IClassProperty property)
        => property.TypeName.IsCollectionTypeName()
            && !string.IsNullOrEmpty(settings.NameSettings.AddMethodNameFormatString);

    private static ClassMethodBuilder CreateCollectionPropertyOverload(ITypeBase instance,
                                                                       ImmutableBuilderClassSettings settings,
                                                                       bool extensionMethod,
                                                                       IClassProperty property,
                                                                       IOverload overload)
        => new ClassMethodBuilder()
            .WithName(string.Format(overload.MethodName.WhenNullOrEmpty(settings.NameSettings.AddMethodNameFormatString), property.Name))
            .ConfigureForExtensionMethod(instance, settings, extensionMethod)
            .AddParameters(overload.Parameters.Select(x => new ParameterBuilder(x)))
            .AddLiteralCodeStatements(GetImmutableBuilderAddOverloadMethodStatements(settings,
                                                                                     property,
                                                                                     overload.InitializeExpression,
                                                                                     extensionMethod));

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
                            property.TypeName.GetGenericArguments(),
                            property.TypeName.GetClassName(),
                            property.TypeName.GetGenericArguments().GetClassName()
                        ).FixCollectionTypeName(typeof(IEnumerable<>).WithoutGenerics())
                    )
                    .WithIsNullable(property.IsNullable)
                    .WithIsValueType(property.IsValueType)
            )
            .AddLiteralCodeStatements(settings.ConstructorSettings.AddNullChecks
                ? $"return {GetCallPrefix(extensionMethod, false)}Add{property.Name}({property.Name.ToPascalCase()}?.ToArray() ?? throw new {typeof(ArgumentNullException).FullName}(\"{property.Name.ToPascalCase()}\"));"
                : $"return {GetCallPrefix(extensionMethod, false)}Add{property.Name}({property.Name.ToPascalCase()}.ToArray());");

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
                            property.TypeName,                                     // 0
                            property.TypeName.GetGenericArguments(),               // 1
                            property.TypeName.GetClassName(),                      // 2
                            property.TypeName.GetGenericArguments().GetClassName() // 3
                        ).ConvertTypeNameToArray()
                    ).WithIsNullable(property.IsNullable).WithIsValueType(property.IsValueType)
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
            property.TypeName,                                     // 0
            property.TypeName.GetGenericArguments(),               // 1
            property.TypeName.GetClassName(),                      // 2
            property.TypeName.GetGenericArguments().GetClassName() // 3
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
                            ? $"System.Func<{typeName.AppendNullableAnnotation(property, settings.TypeSettings.EnableNullableReferenceTypes)}>"
                            : typeName
                    )
                    .WithIsNullable(!useLazyInitialization && property.IsNullable)
                    .WithIsValueType(property.IsValueType)
                    .WithDefaultValue(useLazyInitialization
                        ? null
                        : property.Metadata.GetValue<object?>(MetadataNames.CustomBuilderWithDefaultPropertyValue, () => null))
            )
            .AddLiteralCodeStatements
            (
                string.Format
                (
                    property.Metadata.GetStringValue(MetadataNames.CustomBuilderWithExpression, $"{GetCallPrefix(extensionMethod, useLazyInitialization)}{GetCallPropertyName(property.Name, useLazyInitialization)}{GetCallSuffix(useLazyInitialization)} = {GetExpressionPrefix(property, settings, useLazyInitialization)}{property.Name.ToPascalCase().GetCsharpFriendlyName()}{GetExpressionSuffix(useLazyInitialization)}{GetNullCheckSuffix(property, property.Name.ToPascalCase(), settings.ConstructorSettings.AddNullChecks)};"),
                    property.Name,                                                                   // 0
                    property.Name.ToPascalCase(),                                                    // 1
                    useLazyInitialization                                                            // 2
                        ? $"{property.Name.ToPascalCase().GetCsharpFriendlyName()}Delegate.Invoke()"
                        : property.Name.ToPascalCase().GetCsharpFriendlyName(),
                    property.TypeName,                                                               // 3
                    property.TypeName.GetGenericArguments(),                                         // 4
                    "{",                                                                             // 5
                    "}"                                                                              // 6
                ),
                $"return {GetReturnValue(settings, extensionMethod)};"
            );
    }

    private static string GetNullCheckSuffix(IClassProperty property, string name, bool addNullChecks)
    {
        if (!addNullChecks)
        {
            return string.Empty;
        }

        if (property.IsNullable || property.IsValueType)
        {
            return string.Empty;
        }

        return $" ?? throw new {typeof(ArgumentNullException).FullName}(\"{name}\")";
    }

    private static ClassMethodBuilder CreateSinglePropertyOverload(ITypeBase instance,
                                                                   ImmutableBuilderClassSettings settings,
                                                                   bool extensionMethod,
                                                                   IClassProperty property,
                                                                   IOverload overload)
        => new ClassMethodBuilder()
            .WithName(string.Format(overload.MethodName.WhenNullOrEmpty(settings.NameSettings.SetMethodNameFormatString),
                                    property.Name))
            .ConfigureForExtensionMethod(instance, settings, extensionMethod)
            .AddParameters(overload.Parameters.Select(x => new ParameterBuilder(x)))
            .AddLiteralCodeStatements
            (
                string.Format(overload.InitializeExpression,
                              property.Name.ToPascalCase(),                     // 0
                              property.TypeName.GetCsharpFriendlyTypeName(),    // 1
                              GetPropertyName(property.Name, extensionMethod)), // 2
                $"return {GetReturnValue(settings, extensionMethod)};"
            );

    private static string GetPropertyName(string name, bool extensionMethod)
        => extensionMethod
            ? $"instance.{name}"
            : name;

    private static ClassMethodBuilder ConfigureForExtensionMethod(this ClassMethodBuilder builder,
                                                                  ITypeBase instance,
                                                                  ImmutableBuilderClassSettings settings,
                                                                  bool extensionMethod)
        => builder.WithTypeName(CreateTypeName(instance, settings, extensionMethod && !settings.IsBuilderForAbstractEntity))
                  .AddGenericTypeArguments(new[] { "T" }.Where(_ => extensionMethod && !settings.IsBuilderForAbstractEntity))
                  .AddGenericTypeArgumentConstraints(new[] { $"where T : {string.Format(settings.NameSettings.BuilderNameFormatString, instance.Name)}" }.Where(_ => extensionMethod && !settings.IsBuilderForAbstractEntity))
                  .WithStatic(extensionMethod)
                  .WithExtensionMethod(extensionMethod)
                  .AddParameters((new[]
                  {
                      new ParameterBuilder()
                        .WithName("instance")
                        .WithTypeName(CreateTypeName(instance, settings, extensionMethod && !settings.IsBuilderForAbstractEntity))
                  }).Where(_ => extensionMethod));

    private static string CreateTypeName(ITypeBase instance, ImmutableBuilderClassSettings settings, bool extensionMethod)
    {
        if (extensionMethod)
        {
            return "T";
        }

        if (settings.IsBuilderForAbstractEntity)
        {
            return "TBuilder" + instance.GetGenericTypeArgumentsString();
        }

        return string.Format(settings.NameSettings.BuilderNameFormatString, instance.Name) + instance.GetGenericTypeArgumentsString();
    }

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

    private static IEnumerable<IOverload> GetOverloads(IClassProperty property)
        => property.Metadata.GetValues<IOverload>(MetadataNames.CustomBuilderWithOverload).ToArray();

    private static List<string> GetImmutableBuilderAddMethodStatements(ImmutableBuilderClassSettings settings,
                                                                       IClassProperty property,
                                                                       bool extensionMethod)
    {
        var statements = new List<string>();
        if (settings.ConstructorSettings.AddNullChecks)
        {
            statements.Add($"if ({property.Name.ToPascalCase().GetCsharpFriendlyName()} == null) throw new {typeof(ArgumentNullException).FullName}(\"{property.Name.ToPascalCase()}\");");
            if (settings.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
            {
                statements.Add($"if ({GetCallPrefix(extensionMethod, false)}{property.Name} == null) {GetCallPrefix(extensionMethod, false)}{GetInitializationName(property.Name, settings)} = {GetImmutableBuilderClassConstructorInitializer(settings, property)};");
            }
        }
        statements.Add(string.Format
        (
            property.Metadata.GetStringValue(MetadataNames.CustomBuilderAddExpression, CreateImmutableBuilderCollectionPropertyAddExpression(property, extensionMethod, settings)),
            property.Name.ToPascalCase(),           // 0
            property.TypeName,                      // 1
            property.TypeName.GetGenericArguments() // 2
        ));
        statements.Add($"return {GetReturnValue(settings, extensionMethod)};");

        return statements;
    }

    private static string CreateImmutableBuilderCollectionPropertyAddExpression(IClassProperty property, bool extensionMethod, ImmutableBuilderClassSettings settings)
    {
        if (settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            return $"{GetCallPrefix(extensionMethod, false)}{property.Name} = {property.Name}.Concat({property.Name.ToPascalCase()});";
        }

        return $"{GetCallPrefix(extensionMethod, false)}{property.Name}.AddRange({property.Name.ToPascalCase()});";
    }

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
                $"if ({property.Name.ToPascalCase().GetCsharpFriendlyName()} != null)",
                "{",
                string.Format(overloadExpression,
                              property.Name.ToPascalCase(),                                        // 0
                              property.TypeName.GetCsharpFriendlyTypeName(),                       // 1
                              property.TypeName.GetGenericArguments(),                             // 2
                              CreateIndentForImmutableBuilderAddOverloadMethodStatement(settings), // 3
                              GetPropertyName(property.Name, extensionMethod)),                    // 4
                "}",
                $"return {GetReturnValue(settings, extensionMethod)};"
            }).ToList()
            : (new[]
            {
                string.Format(overloadExpression,
                              property.Name.ToPascalCase(),                                        // 0
                              property.TypeName,                                                   // 1
                              property.TypeName.GetGenericArguments(),                             // 2
                              CreateIndentForImmutableBuilderAddOverloadMethodStatement(settings), // 3
                              GetPropertyName(property.Name, extensionMethod)),                    // 4
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
                        property.TypeName,                                     // 0
                        property.TypeName.GetGenericArguments(),               // 1
                        property.TypeName.GetClassName(),                      // 2
                        property.TypeName.GetGenericArguments().GetClassName() // 3
                    ).FixCollectionTypeName(settings.TypeSettings.NewCollectionTypeName)
                )
                .WithIsNullable(property.IsNullable)
                .WithIsValueType(property.IsValueType)
                .WithParentTypeFullName(property.ParentTypeFullName)
                .AddAttributes(property.Attributes.Where(_ => settings.GenerationSettings.CopyAttributes).Select(x => new AttributeBuilder(x)))
                .AddMetadata(property.Metadata.Select(x => new MetadataBuilder(x)))
                .AddGetterCodeStatements(CreateImmutableBuilderPropertyGetterStatements(property, settings))
                .AddSetterCodeStatements(CreateImmutableBuilderPropertySetterStatements(property, settings))
            );

    private static IEnumerable<ICodeStatementBuilder> CreateImmutableBuilderPropertyGetterStatements(IClassProperty property,
                                                                                                     ImmutableBuilderClassSettings settings)
    {
        if (settings.GenerationSettings.UseLazyInitialization && !property.TypeName.IsCollectionTypeName())
        {
            yield return new LiteralCodeStatementBuilder($"return _{property.Name.ToPascalCase()}Delegate.Value;");
        }
        else if (settings.ConstructorSettings.AddNullChecks && settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            yield return new LiteralCodeStatementBuilder($"return _{property.Name.ToPascalCase()};");
        }
        else if (settings.GenerationSettings.CopyPropertyCode)
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
        if (settings.GenerationSettings.UseLazyInitialization && !property.TypeName.IsCollectionTypeName())
        {
            yield return new LiteralCodeStatementBuilder($"_{property.Name.ToPascalCase()}Delegate = new {GetNewExpression(property, settings)}(() => value);");
        }
        else if (settings.ConstructorSettings.AddNullChecks && settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            yield return new LiteralCodeStatementBuilder($"_{property.Name.ToPascalCase()} = value{GetNullCheckSuffix(property, "value", settings.ConstructorSettings.AddNullChecks)};");
        }
        else if (settings.GenerationSettings.CopyPropertyCode)
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
                                                 p.Name,                           // 0
                                                 p.Name.ToPascalCase(),            // 1
                                                 p.IsNullable
                                                    ? "?"
                                                    : string.Empty,                // 2
                                                 p.TypeName,                       // 3
                                                 p.TypeName.GetGenericArguments(), // 4
                                                 p.IsNullable || !settings.TypeSettings.EnableNullableReferenceTypes
                                                    ? string.Empty
                                                    : "!"))                        // 5
        );
    }
}
