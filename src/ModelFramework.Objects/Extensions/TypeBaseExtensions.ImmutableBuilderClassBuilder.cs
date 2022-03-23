namespace ModelFramework.Objects.Extensions;

public static partial class TypeBaseEtensions
{
    public static IClass ToImmutableBuilderClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.ToImmutableBuilderClassBuilder(settings).Build();

    public static ClassBuilder ToImmutableBuilderClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        if (!instance.Properties.Any())
        {
            throw new InvalidOperationException("To create an immutable builder class, there must be at least one property");
        }

        return new ClassBuilder()
            .WithName(instance.Name + "Builder")
            .WithNamespace(instance.Namespace)
            .WithPartial(instance.Partial)
            .AddConstructors(GetImmutableBuilderClassConstructors(instance, settings))
            .AddMethods(GetImmutableBuilderClassMethods(instance, settings))
            .AddProperties(GetImmutableBuilderClassProperties(instance, settings))
            .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)))
            .AddFields(instance.GetFields().Select(x => new ClassFieldBuilder(x)));
    }

    public static IClass ToBuilderExtensionsClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        => instance.ToBuilderExtensionsClassBuilder(settings).Build();

    public static ClassBuilder ToBuilderExtensionsClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
    {
        if (!instance.Properties.Any())
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

    private static IEnumerable<ClassConstructorBuilder> GetImmutableBuilderClassConstructors(ITypeBase instance,
                                                                                             ImmutableBuilderClassSettings settings)
    {
        yield return new ClassConstructorBuilder()
            .AddLiteralCodeStatements
            (
                instance.Properties
                    .Where(p => p.TypeName.IsCollectionTypeName())
                    .Select(p => $"{p.Name} = new {GetImmutableBuilderClassConstructorInitializer(settings, p)}();")
            )
            .AddLiteralCodeStatements
            (
                instance.Properties
                    .Where(p => settings.ConstructorSettings.SetDefaultValues && !p.TypeName.IsCollectionTypeName() && !p.IsNullable)
                    .Select(p => $"{p.Name} = {p.GetDefaultValue()};")
            );

        if (settings.ConstructorSettings.AddCopyConstructor)
        {
            yield return new ClassConstructorBuilder()
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName("source")
                        .WithTypeName(FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate))
                )
                .AddLiteralCodeStatements
                (
                    instance.Properties
                        .Where(p => p.TypeName.IsCollectionTypeName())
                        .Select(p => $"{p.Name} = new {GetCopyConstructorInitializeExpression(settings, p)}();")
                )
                .AddLiteralCodeStatements
                (
                    instance.Properties.Select(p => p.CreateImmutableBuilderInitializationCode(settings.ConstructorSettings.AddNullChecks) + ";")
                );
        }

        if (settings.ConstructorSettings.AddConstructorWithAllProperties)
        {
            var properties = GetImmutableBuilderConstructorProperties(instance, settings.Poco);

            yield return new ClassConstructorBuilder()
                .AddParameters(properties.Select(p => new ParameterBuilder()
                    .WithName(p.Name.ToPascalCase())
                    .WithTypeName(string.Format
                    (
                        p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixCollectionTypeName(settings.NewCollectionTypeName)),
                        p.Name.ToPascalCase().GetCsharpFriendlyName(),
                        p.TypeName.GetGenericArguments()
                    ))
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
                    instance.Properties.Select
                    (
                        p => p.TypeName.IsCollectionTypeName()
                            ? CreateConstructorStatementForCollection(p, settings)
                            : $"{p.Name} = {p.Name.ToPascalCase().GetCsharpFriendlyName()};"
                    )
                );
        }
    }

    private static IEnumerable<IClassProperty> GetImmutableBuilderConstructorProperties(ITypeBase instance, bool poco)
    {
        var cls = instance as IClass;
        var ctors = cls?.Constructors ?? new ValueCollection<IClassConstructor>();

        if (poco)
        {
            return instance.Properties;
        }
        var ctor = ctors.FirstOrDefault(x => x.Parameters.Count > 0);
        if (ctor == null)
        {
            return instance.Properties;
        }
        return ctor.Parameters
                .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                .Where(x => x != null);
    }

    private static string GetConstructorInitializeExpressionForCollection(ImmutableBuilderClassSettings settings, IClassProperty p)
        => string.Format(p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
                         p.TypeName,
                         p.TypeName.GetGenericArguments()
                        ).FixCollectionTypeName(settings.NewCollectionTypeName)
                         .GetCsharpFriendlyTypeName();

    private static string GetCopyConstructorInitializeExpression(ImmutableBuilderClassSettings settings, IClassProperty p)
        => string.Format(p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
                         p.TypeName,
                         p.TypeName.GetGenericArguments()
                        ).FixCollectionTypeName(settings.NewCollectionTypeName)
                         .GetCsharpFriendlyTypeName();

    private static string GetImmutableBuilderClassConstructorInitializer(ImmutableBuilderClassSettings settings, IClassProperty p)
        => string.Format
        (
            p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
            p.TypeName,
            p.TypeName.GetGenericArguments()
        ).FixCollectionTypeName(settings.NewCollectionTypeName)
         .GetCsharpFriendlyTypeName();

    private static string CreateConstructorStatementForCollection(IClassProperty p, ImmutableBuilderClassSettings settings)
        => settings.ConstructorSettings.AddNullChecks
            ? $"if ({p.Name.ToPascalCase()} != null) {p.Name}.AddRange({p.Name.ToPascalCase()});"
            : $"{p.Name}.AddRange({p.Name.ToPascalCase()});";

    private static IEnumerable<ClassMethodBuilder> GetImmutableBuilderClassMethods(ITypeBase instance,
                                                                                   ImmutableBuilderClassSettings settings)
    {
        var openSign = GetImmutableBuilderPocoOpenSign(settings.Poco);
        var closeSign = GetImmutableBuilderPocoCloseSign(settings.Poco);
        yield return new ClassMethodBuilder()
            .WithName("Build")
            .WithTypeName(FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate))
            .AddLiteralCodeStatements($"return new {FormatInstanceName(instance, true, settings.FormatInstanceTypeNameDelegate)}{openSign}{GetBuildMethodParameters(instance, settings.Poco)}{closeSign};");

        foreach (var classMethodBuilder in GetImmutableBuilderClassPropertyMethods(instance, settings, false))
        {
            yield return classMethodBuilder;
        }
    }

    private static IEnumerable<ClassMethodBuilder> GetImmutableBuilderClassPropertyMethods(ITypeBase instance,
                                                                                           ImmutableBuilderClassSettings settings,
                                                                                           bool extensionMethod)
    {
        if (string.IsNullOrEmpty(settings.SetMethodNameFormatString))
        {
            yield break;
        }

        foreach (var property in instance.Properties)
        {
            var overloads = GetOverloads(property);
            if (property.TypeName.IsCollectionTypeName())
            {
                // collection
                yield return new ClassMethodBuilder()
                    .WithName($"Add{property.Name}")
                    .ConfigureForExtensionMethod(instance, extensionMethod)
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
                                ).FixCollectionTypeName("System.Collections.Generic.IEnumerable")
                            )
                            .WithIsNullable(property.IsNullable)
                    )
                    .AddLiteralCodeStatements($"return {GetCallPrefix(extensionMethod)}Add{property.Name}({property.Name.ToPascalCase()}.ToArray());");
                yield return new ClassMethodBuilder()
                    .WithName($"Add{property.Name}")
                    .ConfigureForExtensionMethod(instance, extensionMethod)
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
                                ).FixTypeName()
                                 .ConvertTypeNameToArray()
                            )
                            .WithIsNullable(property.IsNullable)
                    )
                    .AddLiteralCodeStatements(GetImmutableBuilderAddMethodStatements(settings, property, extensionMethod));

                foreach (var overload in overloads)
                {
                    yield return new ClassMethodBuilder()
                        .WithName(string.Format(overload.MethodName.WhenNullOrEmpty(settings.SetMethodNameFormatString), property.Name))
                        .ConfigureForExtensionMethod(instance, extensionMethod)
                        .AddParameters
                        (
                            new ParameterBuilder()
                                .WithName(property.Name.ToPascalCase())
                                .WithTypeName(string.Format(overload.ArgumentType,
                                                            property.TypeName,
                                                            property.TypeName.GetGenericArguments()).FixCollectionTypeName("System.Collections.Generic.IEnumerable"))
                                .WithIsNullable(property.IsNullable)
                        )
                        .AddLiteralCodeStatements($"return {string.Format(overload.MethodName.WhenNullOrEmpty(settings.SetMethodNameFormatString), property.Name)}({property.Name.ToPascalCase()}.ToArray());");
                    yield return new ClassMethodBuilder()
                        .WithName(string.Format(overload.MethodName.WhenNullOrEmpty(settings.SetMethodNameFormatString),
                                                property.Name))
                        .ConfigureForExtensionMethod(instance, extensionMethod)
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
                }
            }
            else
            {
                // single
                yield return new ClassMethodBuilder()
                .WithName(string.Format(settings.SetMethodNameFormatString,
                                        property.Name))
                .ConfigureForExtensionMethod(instance, extensionMethod)
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
                            )
                        )
                        .WithIsNullable(property.IsNullable)
                        .WithDefaultValue(property.Metadata.GetValue<object?>(MetadataNames.CustomBuilderWithDefaultPropertyValue, () => null))
                )
                .AddLiteralCodeStatements
                (
                    string.Format
                    (
                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderWithExpression, $"{GetCallPrefix(extensionMethod)}{property.Name} = {property.Name.ToPascalCase().GetCsharpFriendlyName()};"),
                        property.Name,
                        property.Name.ToPascalCase(),
                        property.Name.ToPascalCase().GetCsharpFriendlyName(),
                        property.TypeName,
                        property.TypeName.GetGenericArguments(),
                        "{",
                        "}"
                    ),
                    $"return {GetReturnValue(extensionMethod)};"
                );
                foreach (var overload in overloads)
                {
                    yield return new ClassMethodBuilder()
                        .WithName(string.Format(overload.MethodName.WhenNullOrEmpty(settings.SetMethodNameFormatString),
                                                property.Name))
                        .ConfigureForExtensionMethod(instance, extensionMethod)
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
                            $"return {GetReturnValue(extensionMethod)};"
                        );
                }
            }
        }
    }

    private static ClassMethodBuilder ConfigureForExtensionMethod(this ClassMethodBuilder builder, ITypeBase instance, bool extensionMethod)
        => builder.WithTypeName($"{instance.Name}Builder")
                  .WithStatic(extensionMethod)
                  .WithExtensionMethod(extensionMethod)
                  .AddParameters(new[]
                  {
                              new ParameterBuilder().WithName("instance")
                                                    .WithTypeName($"{instance.Name}Builder")
                  }.Where(_ => extensionMethod));

    private static string GetCallPrefix(bool extensionMethod)
        => extensionMethod
            ? "instance."
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
                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderAddExpression, $"    {GetCallPrefix(extensionMethod)}{property.Name}.AddRange({property.Name.ToPascalCase()});"),
                        property.Name.ToPascalCase(),
                        property.TypeName,
                        property.TypeName.GetGenericArguments()
                    ),
                    "}",
                    $"return {GetReturnValue(extensionMethod)};"
                }.ToList()
            : new[]
                {
                    string.Format
                    (
                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderAddExpression, $"{GetCallPrefix(extensionMethod)}{property.Name}.AddRange({property.Name.ToPascalCase()});"),
                        property.Name.ToPascalCase(),
                        property.TypeName,
                        property.TypeName.GetGenericArguments()
                    ),
                    $"return {GetReturnValue(extensionMethod)};"
                }.ToList();

    private static string GetReturnValue(bool extensionMethod)
        => extensionMethod
            ? "instance"
            : "this";

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
                    $"return {GetReturnValue(extensionMethod)};"
            }).ToList()
            : (new[]
            {
                string.Format(overloadExpression,
                              property.Name.ToPascalCase(),
                              property.TypeName.FixTypeName(),
                              property.TypeName.GetGenericArguments(),
                              CreateIndentForImmutableBuilderAddOverloadMethodStatement(settings),
                              property.Name),
                $"return {GetReturnValue(extensionMethod)};"
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

        var ns = string.IsNullOrEmpty(instance.Namespace)
            ? string.Empty
            : instance.Namespace + ".";

        return (ns + instance.Name).GetCsharpFriendlyTypeName();
    }

    private static IEnumerable<ClassPropertyBuilder> GetImmutableBuilderClassProperties(ITypeBase instance,
                                                                                        ImmutableBuilderClassSettings settings)
        => instance.Properties.Select(property => new ClassPropertyBuilder()
            .WithName(property.Name)
            .WithTypeName
            (
                string.Format
                (
                    property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                    property.TypeName,
                    property.TypeName.GetGenericArguments()
                ).FixCollectionTypeName(settings.NewCollectionTypeName)
            )
            .WithIsNullable(property.IsNullable)
            .AddAttributes(property.Attributes.Select(x => new AttributeBuilder(x)))
            .AddMetadata(property.Metadata.Select(x => new MetadataBuilder(x)))
            .AddGetterCodeStatements(property.GetterCodeStatements.Select(x => x.CreateBuilder()))
            .AddSetterCodeStatements(property.SetterCodeStatements.Select(x => x.CreateBuilder()))
        );

    private static string GetBuildMethodParameters(ITypeBase instance, bool poco)
    {
        var properties = GetImmutableBuilderConstructorProperties(instance, poco);

        var defaultValueDelegate = poco
            ? new Func<IClassProperty, string>(p => $"{p.Name} = {p.Name}")
            : new Func<IClassProperty, string>(p => $"{p.Name}");

        return string.Join
        (
            ", ",
            properties.Select(p => string.Format(p.Metadata.GetStringValue(MetadataNames.CustomBuilderMethodParameterExpression, defaultValueDelegate(p)),
                                                 p.Name,
                                                 p.Name.ToPascalCase()))
        );
   
    }
}
