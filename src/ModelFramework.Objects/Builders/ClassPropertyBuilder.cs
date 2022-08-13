namespace ModelFramework.Objects.Builders;

public partial class ClassPropertyBuilder
{
    public ClassPropertyBuilder ConvertCollectionOnBuilderToEnumerable(bool addNullChecks, string collectionType = "System.Collections.Generic.List")
        => AddMetadata(MetadataNames.CustomImmutableArgumentType, "System.Collections.Generic.IEnumerable<{1}>")
          .AddMetadata(MetadataNames.CustomImmutableBuilderDefaultValue, addNullChecks
            ? "new " + collectionType + "<{1}>({0} ?? Enumerable.Empty<{1}>())"
            : "new " + collectionType + "<{1}>({0})")
          .AddMetadata(MetadataNames.CustomImmutableDefaultValue, addNullChecks
            ? "new " + collectionType + "<{1}>({0} ?? Enumerable.Empty<{1}>())"
            : "new " + collectionType + "<{1}>({0})");

    public ClassPropertyBuilder ConvertSinglePropertyToBuilderOnBuilder(string? argumentType = null,
                                                                        string? customBuilderConstructorInitializeExpression = null,
                                                                        string? customBuilderMethodParameterExpression = null,
                                                                        bool addNullableCheck = true)
        => AddMetadata(MetadataNames.CustomBuilderArgumentType, argumentType ?? "{0}Builder")
          .AddMetadata(MetadataNames.CustomBuilderMethodParameterExpression, customBuilderMethodParameterExpression ?? (IsNullable || addNullableCheck
                ? "{0}?.Build()"
                : "{0}.Build()"))
          .AddMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression ?? (argumentType == null ? "{0} = new {2}Builder(source.{0})" : "{0} = new " + argumentType + "(source.{0})"));

    public ClassPropertyBuilder ConvertCollectionPropertyToBuilderOnBuilder(bool addNullChecks,
                                                                            string collectionType = "System.Collections.Generic.List",
                                                                            string? argumentType = null,
                                                                            string? customBuilderConstructorInitializeExpression = null)
        => ConvertCollectionOnBuilderToEnumerable(addNullChecks, collectionType)
          .AddMetadata(MetadataNames.CustomBuilderArgumentType, argumentType ?? "System.Collections.Generic.IEnumerable<{1}Builder>")
          .AddMetadata(MetadataNames.CustomBuilderMethodParameterExpression, "{0}.Select(x => x.Build())")
          .AddMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression ?? (argumentType == null ? "{4}{0}.AddRange(source.{0}.Select(x => new {3}Builder(x)))" : "{4}{0}.AddRange(source.{0}.Select(x => new " + argumentType.GetGenericArguments() + "(x)))"));

    public ClassPropertyBuilder AddBuilderOverload(string methodNameTemplate,
                                               Type parameterType,
                                               string parameterNameTemplate,
                                               string initializeExpression)
        => AddBuilderOverload(methodNameTemplate,
                              parameterType,
                              parameterNameTemplate,
                              false,
                              initializeExpression);

    public ClassPropertyBuilder AddBuilderOverload(string methodNameTemplate,
                                                   Type parameterType,
                                                   string parameterNameTemplate,
                                                   bool parameterTypeNullable,
                                                   string initializeExpression)
        => AddBuilderOverload(methodNameTemplate,
                              new[] { parameterType.AssemblyQualifiedName },
                              new[] { parameterNameTemplate },
                              new[] { parameterTypeNullable },
                              initializeExpression);

    public ClassPropertyBuilder AddBuilderOverload(string methodNameTemplate,
                                                   Type[] parameterTypes,
                                                   string[] parameterNameTemplates,
                                                   bool[] parameterTypeNullables,
                                                   string initializeExpression)
        => AddBuilderOverload(methodNameTemplate,
                              parameterTypes.Select(x => x.AssemblyQualifiedName).ToArray(),
                              parameterNameTemplates,
                              parameterTypeNullables,
                              initializeExpression);

    public ClassPropertyBuilder AddBuilderOverload(string methodNameTemplate,
                                                   Type[] parameterTypes,
                                                   string[] parameterNameTemplates,
                                                   string initializeExpression)
        => AddBuilderOverload(methodNameTemplate,
                              parameterTypes,
                              parameterNameTemplates,
                              parameterTypes.Select(_ => false).ToArray(),
                              initializeExpression);

    public ClassPropertyBuilder AddBuilderOverload(string methodNameTemplate,
                                                   string parameterTypeName,
                                                   string parameterNameTemplate,
                                                   bool parameterTypeNullable,
                                                   string initializeExpression)
        => AddBuilderOverload(methodNameTemplate,
                              new[] { parameterTypeName },
                              new[] { parameterNameTemplate },
                              new[] { parameterTypeNullable },
                              initializeExpression);

    public ClassPropertyBuilder AddBuilderOverload(string methodNameTemplate,
                                                   string parameterTypeName,
                                                   string parameterNameTemplate,
                                                   string initializeExpression)
        => AddBuilderOverload(methodNameTemplate,
                              parameterTypeName,
                              parameterNameTemplate,
                              false,
                              initializeExpression);

    public ClassPropertyBuilder AddBuilderOverload(string methodNameTemplate,
                                                   string[] parameterTypeNames,
                                                   string[] parameterNameTemplates,
                                                   bool[] parameterTypeNullables,
                                                   string initializeExpression)
        => AddMetadata(MetadataNames.CustomBuilderWithOverloadMethodName, methodNameTemplate)
          .AddMetadata(MetadataNames.CustomBuilderWithOverloadArgumentTypes, parameterTypeNames)
          .AddMetadata(MetadataNames.CustomBuilderWithOverloadArgumentTypeNullables, parameterTypeNullables)
          .AddMetadata(MetadataNames.CustomBuilderWithOverloadArgumentNames, parameterNameTemplates)
          .AddMetadata(MetadataNames.CustomBuilderWithOverloadInitializeExpression, initializeExpression);

    public ClassPropertyBuilder SetDefaultArgumentValueForWithMethod(object defaultValue)
        => AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithDefaultPropertyValue)
                                            .WithValue(defaultValue));

    public ClassPropertyBuilder SetDefaultValueForBuilderClassConstructor(object defaultValue)
        => AddMetadata(MetadataNames.CustomImmutableBuilderDefaultValue, defaultValue);

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
        => this.Chain(() =>  Metadata.Replace(name, newValue));
}
