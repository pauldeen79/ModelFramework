namespace ModelFramework.Objects.Builders;

public partial class ClassPropertyBuilder
{
    public ClassPropertyBuilder ConvertCollectionOnBuilderToEnumerable(bool addNullChecks,
                                                                       string collectionType = "System.Collections.Generic.List")
        => AddMetadata(MetadataNames.CustomImmutableArgumentType, "System.Collections.Generic.IEnumerable<{1}>")
          .AddMetadata(MetadataNames.CustomImmutableBuilderDefaultValue, CreateImmutableBuilderDefaultValue(addNullChecks, collectionType))
          .AddMetadata(MetadataNames.CustomImmutableDefaultValue, CreateImmutableDefaultValue(addNullChecks, collectionType));

    private static string CreateImmutableDefaultValue(bool addNullChecks, string collectionType)
        => collectionType switch
        {
            "System.Collections.Generic.IEnumerable" => "{0} ?? Enumerable.Empty<{1}>()",
            _ => addNullChecks
                ? "new " + collectionType + "<{1}>({0} ?? Enumerable.Empty<{1}>())"
                : "new " + collectionType + "<{1}>({0})"
        };

    private static string CreateImmutableBuilderDefaultValue(bool addNullChecks, string collectionType)
        => collectionType switch
        {
            "System.Collections.Generic.IEnumerable" => "{0} ?? Enumerable.Empty<{1}>()",
            _ => addNullChecks
                ? "new " + collectionType + "<{1}>({0} ?? Enumerable.Empty<{1}>())"
                : "new " + collectionType + "<{1}>({0})"
        };

    public ClassPropertyBuilder ConvertSinglePropertyToBuilderOnBuilder(string? argumentType = null,
                                                                        string? customBuilderConstructorInitializeExpression = null,
                                                                        string? customBuilderMethodParameterExpression = null,
                                                                        bool addNullableCheck = true,
                                                                        bool useLazyInitialization = false,
                                                                        bool useTargetTypeNewExpressions = false,
                                                                        string? buildersNamespace = null)
        => AddMetadata(MetadataNames.CustomBuilderArgumentType, argumentType.WhenNullOrEmpty(() => string.IsNullOrEmpty(buildersNamespace) ? "{0}Builder" : buildersNamespace + ".{2}Builder"))
          .AddMetadata(MetadataNames.CustomBuilderMethodParameterExpression, customBuilderMethodParameterExpression.WhenNullOrEmpty(() => IsNullable || addNullableCheck
                ? "{0}?.Build()"
                : "{0}{2}.Build()"))
          .AddMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression.WhenNullOrEmpty(() => CreateDefaultCustomBuilderConstructorSinglePropertyInitializeExpression(argumentType, useLazyInitialization, useTargetTypeNewExpressions, buildersNamespace)));

    public ClassPropertyBuilder ConvertStringPropertyToStringBuilderPropertyOnBuilder()
    {
        ConvertSinglePropertyToBuilderOnBuilder
        (
            argumentType: typeof(StringBuilder).FullName,
            customBuilderMethodParameterExpression: "{0}?.ToString()",
            customBuilderConstructorInitializeExpression: "_{1}Delegate = new (() => new System.Text.StringBuilder(source.{0}))"
        );
        SetDefaultValueForStringPropertyOnBuilderClassConstructor();
        AddBuilderOverload(new OverloadBuilder().AddParameter("value", typeof(string)).WithInitializeExpression("{2}.Clear().Append(value);").Build());
        AddBuilderOverload(new OverloadBuilder().WithMethodName("AppendTo{0}").AddParameter("value", typeof(string)).WithInitializeExpression("{2}.Append(value);").Build());
        AddBuilderOverload(new OverloadBuilder().WithMethodName("AppendLineTo{0}").AddParameter("value", typeof(string)).WithInitializeExpression("{2}.AppendLine(value);").Build());

        return this;
    }

    private static string CreateDefaultCustomBuilderConstructorSinglePropertyInitializeExpression(string? argumentType,
                                                                                                  bool useLazyInitialization,
                                                                                                  bool useTargetTypeNewExpressions,
                                                                                                  string? buildersNamespace)
    {
        if (useLazyInitialization)
        {
            if (useTargetTypeNewExpressions)
            {
                if (!string.IsNullOrEmpty(buildersNamespace))
                {
                    return "_{1}Delegate = new (() => new " + buildersNamespace + ".{5}Builder(source.{0}))";
                }

                return string.IsNullOrEmpty(argumentType)
                    ? "_{1}Delegate = new (() => new {2}Builder(source.{0}))"
                    : "_{1}Delegate = new (() => new " + argumentType + "(source.{0}))";
            }

            if (!string.IsNullOrEmpty(buildersNamespace))
            {
                return "_{1}Delegate = new System.Lazy<" + buildersNamespace + ".{5}Builder>(() => new " + buildersNamespace + ".{5}Builder(source.{0}))";
            }

            return string.IsNullOrEmpty(argumentType)
                ? "_{1}Delegate = new System.Lazy<{2}Builder>(() => new {2}Builder(source.{0}))"
                : "_{1}Delegate = new System.Lazy<" + argumentType + ">(() => new " + argumentType + "(source.{0}))";
        }

        if (!string.IsNullOrEmpty(buildersNamespace))
        {
            return "{0} = new " + buildersNamespace + ".{5}Builder(source.{0})";
        }

        return string.IsNullOrEmpty(argumentType)
            ? "{0} = new {2}Builder(source.{0})"
            : "{0} = new " + argumentType + "(source.{0})";
    }

    public ClassPropertyBuilder ConvertCollectionPropertyToBuilderOnBuilder(bool addNullChecks,
                                                                            string collectionType = "System.Collections.Generic.List",
                                                                            string? argumentType = null,
                                                                            string? customBuilderConstructorInitializeExpression = null,
                                                                            string? buildersNamespace = null,
                                                                            string? customBuilderMethodParameterExpression = null,
                                                                            string? builderCollectionTypeName = null)
        => ConvertCollectionOnBuilderToEnumerable(addNullChecks, collectionType)
          .AddMetadata(MetadataNames.CustomBuilderArgumentType, argumentType.WhenNullOrEmpty(() => string.IsNullOrEmpty(buildersNamespace) ? "System.Collections.Generic.IEnumerable<{1}Builder>" : "System.Collections.Generic.IEnumerable<" + buildersNamespace + ".{3}Builder>"))
          .AddMetadata(MetadataNames.CustomBuilderMethodParameterExpression, customBuilderMethodParameterExpression.WhenNullOrEmpty("{0}.Select(x => x.Build())"))
          .AddMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression.WhenNullOrEmpty(() => CreateDefaultCustomBuilderConstructorCollectionPropertyInitializeExpression(argumentType, buildersNamespace, builderCollectionTypeName)));

    public ClassPropertyBuilder AddCollectionBackingFieldOnImmutableClass(Type collectionType)
    {
        AddMetadata(MetadataNames.CustomImmutablePropertyGetterStatement, new LiteralCodeStatement($"return _{Name.ToString().ToPascalCase()};", Enumerable.Empty<IMetadata>()));
        AddMetadata(MetadataNames.CustomImmutableConstructorInitialization, $"_{Name.ToString().ToPascalCase()} = new {typeof(ValueCollection<>).WithoutGenerics()}<{TypeName.ToString().GetGenericArguments()}>({Name.ToString().ToPascalCase()});");
        AddMetadata(MetadataNames.CustomImmutableBackingField, new ClassFieldBuilder().WithName($"_{Name.ToString().ToPascalCase()}").WithTypeName($"{typeof(ValueCollection<>).WithoutGenerics()}<{TypeName.ToString().GetGenericArguments()}>").Build());
        AddMetadata(MetadataNames.CustomImmutableHasSetter, false);

        return this;
    }

    private static string CreateDefaultCustomBuilderConstructorCollectionPropertyInitializeExpression(string? argumentType,
                                                                                                      string? buildersNamespace,
                                                                                                      string? builderCollectionTypeName)
    {
        if (builderCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            if (buildersNamespace != null && buildersNamespace.Length > 0)
            {
                return "{4}{0} = source.{0}.Select(x => new " + buildersNamespace + ".{6}Builder(x))";
            }

            if (argumentType != null && argumentType.Length > 0)
            {
                return "{4}{0} = source.{0}.Select(x => new " + argumentType.GetGenericArguments() + "(x))";
            }

            return "{4}{0} = source.{0}.Select(x => new {3}Builder(x))";
        }

        if (buildersNamespace != null && buildersNamespace.Length > 0)
        {
            return "{4}{0}.AddRange(source.{0}.Select(x => new " + buildersNamespace + ".{6}Builder(x)))";
        }

        if (argumentType != null && argumentType.Length > 0)
        {
            return "{4}{0}.AddRange(source.{0}.Select(x => new " + argumentType.GetGenericArguments() + "(x)))";
        }

        return "{4}{0}.AddRange(source.{0}.Select(x => new {3}Builder(x)))";
    }

    public ClassPropertyBuilder AddBuilderOverload(IOverload overload)
        => AddMetadata(MetadataNames.CustomBuilderWithOverload, overload);

    public ClassPropertyBuilder SetDefaultArgumentValueForWithMethod(object defaultValue)
        => AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithDefaultPropertyValue)
                                            .WithValue(defaultValue));

    public ClassPropertyBuilder SetDefaultValueForBuilderClassConstructor(object defaultValue)
        => AddMetadata(MetadataNames.CustomImmutableBuilderDefaultValue, defaultValue);

    public ClassPropertyBuilder SetDefaultValueForStringPropertyOnBuilderClassConstructor()
        => SetDefaultValueForBuilderClassConstructor(!IsNullable
            ? new Literal("new System.Text.StringBuilder()")
            : new Literal("default"));

    public ClassPropertyBuilder SetBuilderWithExpression(string expression)
        => AddMetadata(MetadataNames.CustomBuilderWithExpression, expression);

    public ClassPropertyBuilder AddGetterLiteralCodeStatements(params string[] statements)
        => AddGetterCodeStatements(statements.ToLiteralCodeStatementBuilders());

    public ClassPropertyBuilder AddSetterLiteralCodeStatements(params string[] statements)
        => AddSetterCodeStatements(statements.ToLiteralCodeStatementBuilders());

    public ClassPropertyBuilder AddInitializerLiteralCodeStatements(params string[] statements)
        => AddInitializerCodeStatements(statements.ToLiteralCodeStatementBuilders());

    public ClassPropertyBuilder AsReadOnly()
        => WithHasSetter(false);

    public ClassPropertyBuilder WithCustomGetterModifiers(string? customModifiers)
        => ReplaceMetadata(MetadataNames.PropertyGetterModifiers, customModifiers);

    public ClassPropertyBuilder WithCustomSetterModifiers(string? customModifiers)
        => ReplaceMetadata(MetadataNames.PropertySetterModifiers, customModifiers);

    public ClassPropertyBuilder WithCustomInitializerModifiers(string? customModifiers)
        => ReplaceMetadata(MetadataNames.PropertyInitializerModifiers, customModifiers);

    public ClassPropertyBuilder ReplaceMetadata(string name, object? newValue)
        => this.Chain(() => Metadata.Replace(name, newValue));
}
