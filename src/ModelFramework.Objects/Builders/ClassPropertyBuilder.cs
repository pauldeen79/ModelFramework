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
                                                                        bool addNullableCheck = true,
                                                                        bool useLazyInitialization = false,
                                                                        bool useTargetTypeNewExpressions = false)
        => AddMetadata(MetadataNames.CustomBuilderArgumentType, argumentType ?? "{0}Builder")
          .AddMetadata(MetadataNames.CustomBuilderMethodParameterExpression, customBuilderMethodParameterExpression ?? (IsNullable || addNullableCheck
                ? "{0}?.Build()"
                : "{0}.Build()"))
          .AddMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression ?? CreateDefaultCustomBuilderConstructorSinglePropertyInitializeExpression(argumentType, useLazyInitialization, useTargetTypeNewExpressions));

    private static string CreateDefaultCustomBuilderConstructorSinglePropertyInitializeExpression(string? argumentType,
                                                                                                  bool useLazyInitialization,
                                                                                                  bool useTargetTypeNewExpressions)
    {
        if (useLazyInitialization)
        {
            if (useTargetTypeNewExpressions)
            {
                return argumentType == null
                    ? "_{1}Delegate = new (() => new {2}Builder(source.{0}))"
                    : "_{1}Delegate = new (() => new " + argumentType + "(source.{0}))";
            }

            return argumentType == null
                ? "_{1}Delegate = new System.Lazy<{2}Builder>(() => new {2}Builder(source.{0}))"
                : "_{1}Delegate = new System.Lazy<" + argumentType + ">(() => new " + argumentType + "(source.{0}))";
        }

        return argumentType == null
            ? "{0} = new {2}Builder(source.{0})"
            : "{0} = new " + argumentType + "(source.{0})";
    }

    public ClassPropertyBuilder ConvertCollectionPropertyToBuilderOnBuilder(bool addNullChecks,
                                                                            string collectionType = "System.Collections.Generic.List",
                                                                            string? argumentType = null,
                                                                            string? customBuilderConstructorInitializeExpression = null)
        => ConvertCollectionOnBuilderToEnumerable(addNullChecks, collectionType)
          .AddMetadata(MetadataNames.CustomBuilderArgumentType, argumentType ?? "System.Collections.Generic.IEnumerable<{1}Builder>")
          .AddMetadata(MetadataNames.CustomBuilderMethodParameterExpression, "{0}.Select(x => x.Build())")
          .AddMetadata(MetadataNames.CustomBuilderConstructorInitializeExpression, customBuilderConstructorInitializeExpression ?? (argumentType == null ? "{4}{0}.AddRange(source.{0}.Select(x => new {3}Builder(x)))" : "{4}{0}.AddRange(source.{0}.Select(x => new " + argumentType.GetGenericArguments() + "(x)))"));

    public ClassPropertyBuilder AddBuilderOverload(IOverload overload)
        => AddMetadata(MetadataNames.CustomBuilderWithOverload, overload);

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
        => this.Chain(() => Metadata.Replace(name, newValue));
}
