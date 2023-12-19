namespace ModelFramework.Objects.Builders;

public partial class ClassPropertyBuilder
{
    public ClassPropertyBuilder ConvertCollectionOnBuilderToEnumerable(bool addNullChecks,
                                                                       ArgumentValidationType argumentValidationType = ArgumentValidationType.None,
                                                                       string collectionType = "System.Collections.Generic.List")
        => AddMetadata(MetadataNames.CustomImmutableArgumentType, "System.Collections.Generic.IEnumerable<{1}>")
          .AddMetadata(MetadataNames.CustomBuilderDefaultValue, CreateImmutableBuilderDefaultValue(addNullChecks, collectionType))
          .AddMetadata(MetadataNames.CustomImmutableDefaultValue, CreateImmutableDefaultValue(addNullChecks, argumentValidationType, collectionType));

    public ClassPropertyBuilder ConvertSinglePropertyToBuilderOnBuilder(string? argumentType = null,
                                                                        string? customBuilderConstructorInitializeExpression = null,
                                                                        string? customBuilderMethodParameterExpression = null,
                                                                        bool addNullableCheck = true,
                                                                        bool useLazyInitialization = false,
                                                                        bool useTargetTypeNewExpressions = false,
                                                                        string? buildersNamespace = null)
    => AddMetadata(MetadataNames.CustomBuilderArgumentType, argumentType.WhenNullOrEmpty(() => string.IsNullOrEmpty(buildersNamespace) ? "{0}Builder" : buildersNamespace + ".{2}Builder"))
      .AddMetadata(MetadataNames.CustomBuilderMethodParameterExpression, customBuilderMethodParameterExpression.WhenNullOrEmpty(() => IsNullable || addNullableCheck
            ? "{0}?.Build(){5}"
            : "{0}{2}.Build(){5}"))
      .AddMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression.WhenNullOrEmpty(() => CreateDefaultCustomBuilderConstructorSinglePropertyInitializeExpression(argumentType, useLazyInitialization, useTargetTypeNewExpressions, buildersNamespace)));

    public ClassPropertyBuilder WithCustomBuilderConstructorInitializeExpressionSingleProperty(string? argumentType = null,
                                                                                               string? customBuilderConstructorInitializeExpression = null,
                                                                                               bool useLazyInitialization = false,
                                                                                               bool useTargetTypeNewExpressions = false,
                                                                                               string? buildersNamespace = null)
        => ReplaceMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression
            .WhenNullOrEmpty(() => CreateDefaultCustomBuilderConstructorSinglePropertyInitializeExpression(argumentType, useLazyInitialization, useTargetTypeNewExpressions, buildersNamespace)));

    public ClassPropertyBuilder WithCustomBuilderArgumentTypeSingleProperty(string? argumentType = null,
                                                                            string? buildersNamespace = null,
                                                                            string builderName = "Builder")
        => ReplaceMetadata(MetadataNames.CustomBuilderArgumentType, argumentType.WhenNullOrEmpty(() => string.IsNullOrEmpty(buildersNamespace) ? "{0}" + builderName : buildersNamespace + ".{2}" + builderName));

    public ClassPropertyBuilder WithCustomBuilderMethodParameterExpression(string? customBuilderMethodParameterExpression = null,
                                                                           string buildMethodName = "Build",
                                                                           bool addNullableCheck = true)
        => ReplaceMetadata(MetadataNames.CustomBuilderMethodParameterExpression, customBuilderMethodParameterExpression.WhenNullOrEmpty(() => IsNullable || addNullableCheck
            ? "{0}?." + buildMethodName + "(){5}"
            : "{0}{2}." + buildMethodName + "(){5}"));

    public ClassPropertyBuilder ConvertStringPropertyToStringBuilderPropertyOnBuilder(bool useLazyInitialization)
    {
        var argumentType = typeof(StringBuilder).FullName;
        var customBuilderMethodParameterExpression = "{0}?.ToString()";
        var customBuilderConstructorInitializeExpression = useLazyInitialization
            ? $"_{{1}}Delegate = new (() => new {typeof(StringBuilder).FullName}(source.{{0}}))"
            : $"{{0}} = new {typeof(StringBuilder).FullName}(source.{{0}})";

        WithCustomBuilderConstructorInitializeExpressionSingleProperty(argumentType, customBuilderConstructorInitializeExpression, useLazyInitialization);
        WithCustomBuilderArgumentTypeSingleProperty(argumentType);
        WithCustomBuilderMethodParameterExpression(customBuilderMethodParameterExpression);

        SetDefaultValueForStringPropertyOnBuilderClassConstructor();
        AddBuilderOverload(new OverloadBuilder().AddParameter("value", typeof(string)).WithInitializeExpression($"if ({{2}} == null)\r\n    {{2}} = new {typeof(StringBuilder).FullName}();\r\n{{2}}.Clear().Append(value);").Build());
        AddBuilderOverload(new OverloadBuilder().WithMethodName("AppendTo{0}").AddParameter("value", typeof(string)).WithInitializeExpression($"if ({{2}} == null)\r\n    {{2}} = new {typeof(StringBuilder).FullName}();\r\n{{2}}.Append(value);").Build());
        AddBuilderOverload(new OverloadBuilder().WithMethodName("AppendLineTo{0}").AddParameter("value", typeof(string)).WithInitializeExpression($"if ({{2}} == null)\r\n    {{2}} = new {typeof(StringBuilder).FullName}();\r\n{{2}}.AppendLine(value);").Build());

        return this;
    }

    public ClassPropertyBuilder ConvertCollectionPropertyToBuilderOnBuilder(bool addNullChecks,
                                                                            string collectionType = "System.Collections.Generic.List",
                                                                            string? argumentType = null,
                                                                            string? customBuilderConstructorInitializeExpression = null,
                                                                            string? buildersNamespace = null,
                                                                            string? customBuilderMethodParameterExpression = null,
                                                                            string? builderCollectionTypeName = null)
        => ConvertCollectionOnBuilderToEnumerable(addNullChecks, collectionType: collectionType)
          .AddMetadata(MetadataNames.CustomBuilderArgumentType, argumentType.WhenNullOrEmpty(() => string.IsNullOrEmpty(buildersNamespace)
            ? "System.Collections.Generic.IEnumerable<{1}Builder>"
            : "System.Collections.Generic.IEnumerable<" + buildersNamespace + ".{3}Builder>"))
          .AddMetadata(MetadataNames.CustomBuilderMethodParameterExpression, customBuilderMethodParameterExpression.WhenNullOrEmpty("{0}.Select(x => x.Build())"))
          .AddMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression.WhenNullOrEmpty(() => CreateDefaultCustomBuilderConstructorCollectionPropertyInitializeExpression(argumentType, buildersNamespace, builderCollectionTypeName)));

    public ClassPropertyBuilder ConvertCollectionPropertyToBuilderOnBuilder(string? argumentType = null,
                                                                            string? customBuilderConstructorInitializeExpression = null,
                                                                            string? buildersNamespace = null,
                                                                            string? customBuilderMethodParameterExpression = null,
                                                                            string? builderCollectionTypeName = null,
                                                                            string builderName = "Builder",
                                                                            string buildMethodName = "Build")
        => AddMetadata(MetadataNames.CustomBuilderArgumentType, argumentType.WhenNullOrEmpty(() => string.IsNullOrEmpty(buildersNamespace)
            ? "System.Collections.Generic.IEnumerable<{1}" + builderName + ">"
            : "System.Collections.Generic.IEnumerable<" + buildersNamespace + ".{3}" + builderName + ">"))
          .AddMetadata(MetadataNames.CustomBuilderMethodParameterExpression, customBuilderMethodParameterExpression.WhenNullOrEmpty("{0}.Select(x => x." + buildMethodName + "())"))
          .AddMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression.WhenNullOrEmpty(() => CreateDefaultCustomBuilderConstructorCollectionPropertyInitializeExpression(argumentType, buildersNamespace, builderCollectionTypeName)));

    public ClassPropertyBuilder AddCollectionBackingFieldOnImmutableClass(Type collectionType, string? propertyGetStatement = null, bool forceNullCheck = false)
    {
        AddMetadata(MetadataNames.CustomImmutablePropertyGetterStatement, new LiteralCodeStatement(propertyGetStatement?.Replace("[Name]", Name).Replace("[NamePascal]", Name.ToPascalCase()) ?? $"return _{Name};", Enumerable.Empty<IMetadata>()));
        var nullSuffix = IsNullable
            ? string.Empty
            : "!";
        AddMetadata(MetadataNames.CustomImmutableConstructorInitialization, IsNullable || forceNullCheck
            ? $"_{Name.ToPascalCase()} = {Name.ToPascalCase()} == null ? null{nullSuffix} : _{Name.ToPascalCase()} = new {collectionType.WithoutGenerics()}<{TypeName.GetGenericArguments()}>({Name.ToPascalCase()});"
            : $"_{Name.ToPascalCase()} = new {collectionType.WithoutGenerics()}<{TypeName.GetGenericArguments()}>({Name.ToPascalCase()});");
        AddMetadata(MetadataNames.CustomImmutableBackingField, new ClassFieldBuilder().WithName($"_{Name.ToPascalCase()}").WithTypeName($"{collectionType.WithoutGenerics()}<{TypeName.GetGenericArguments()}>").WithIsNullable(IsNullable).Build());
        AddMetadata(MetadataNames.CustomImmutableHasSetter, false);

        return this;
    }

    public ClassPropertyBuilder AddBuilderOverload(IOverload overload)
        => AddMetadata(MetadataNames.CustomBuilderWithOverload, overload);

    public ClassPropertyBuilder SetDefaultArgumentValueForWithMethod(object defaultValue)
        => AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithDefaultPropertyValue)
                                            .WithValue(defaultValue));

    public ClassPropertyBuilder SetDefaultValueForBuilderClassConstructor(object defaultValue)
        => AddMetadata(MetadataNames.CustomBuilderDefaultValue, defaultValue);

    public ClassPropertyBuilder SetDefaultValueForStringPropertyOnBuilderClassConstructor()
        => SetDefaultValueForBuilderClassConstructor(!IsNullable
            ? new Literal($"new {typeof(StringBuilder).FullName}()")
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

    public ClassPropertyBuilder AsWritable()
        => WithHasSetter();

    public ClassPropertyBuilder WithCustomGetterModifiers(string? customModifiers)
        => ReplaceMetadata(MetadataNames.PropertyGetterModifiers, customModifiers);

    public ClassPropertyBuilder WithCustomSetterModifiers(string? customModifiers)
        => ReplaceMetadata(MetadataNames.PropertySetterModifiers, customModifiers);

    public ClassPropertyBuilder WithCustomInitializerModifiers(string? customModifiers)
        => ReplaceMetadata(MetadataNames.PropertyInitializerModifiers, customModifiers);

    public ClassPropertyBuilder ReplaceMetadata(string name, object? newValue)
        => this.Chain(() => Metadata.Replace(name, newValue));

    public override string ToString() => !string.IsNullOrEmpty(ParentTypeFullName)
        ? $"{TypeName} {ParentTypeFullName}.{Name}"
        : $"{TypeName} {Name}";

    private static string CreateImmutableDefaultValue(bool addNullChecks, ArgumentValidationType argumentValidationType, string collectionType)
        => collectionType switch
        {
            "System.Collections.Generic.IEnumerable" => "{0} ?? Enumerable.Empty<{1}>()",
            _ => addNullChecks && (argumentValidationType == ArgumentValidationType.Shared || argumentValidationType == ArgumentValidationType.DomainOnly)
                ? "{0} == null ? null{2} : new " + collectionType + "<{1}>({0})"
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
                    return "_{1}Delegate = new (() => new " + buildersNamespace + ".{10}Builder{9}(source.{0}))";
                }

                return string.IsNullOrEmpty(argumentType)
                    ? "_{1}Delegate = new (() => new {2}Builder(source.{0}))"
                    : "_{1}Delegate = new (() => new " + argumentType + "(source.{0}))";
            }

            if (!string.IsNullOrEmpty(buildersNamespace))
            {
                return "_{1}Delegate = new System.Lazy<" + buildersNamespace + ".{10}Builder{9}>(() => new " + buildersNamespace + ".{10}Builder{9}(source.{0}))";
            }

            return string.IsNullOrEmpty(argumentType)
                ? "_{1}Delegate = new System.Lazy<{2}Builder>(() => new {2}Builder(source.{0}))"
                : "_{1}Delegate = new System.Lazy<" + argumentType + ">(() => new " + argumentType + "(source.{0}))";
        }

        if (!string.IsNullOrEmpty(buildersNamespace))
        {
            return "{0} = new " + buildersNamespace + ".{10}Builder{9}(source.{0})";
        }

        return string.IsNullOrEmpty(argumentType)
            ? "{0} = new {2}Builder(source.{0})"
            : "{0} = new " + argumentType + "(source.{0})";
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
}
