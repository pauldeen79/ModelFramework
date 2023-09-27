namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public abstract class ModelFrameworkModelClassBase : ModelFrameworkCSharpClassBase
{
    protected override string SetMethodNameFormatString => string.Empty;
    protected override string AddMethodNameFormatString => string.Empty;
    protected override string BuilderBuildMethodName => "ToEntity";
    protected override string BuilderBuildTypedMethodName => "ToTypedEntity";
    protected override string BuilderNameFormatString => "{0}Model";
    protected override bool UseLazyInitialization => false;

    protected override void FixImmutableBuilderProperty(string name, ClassPropertyBuilder property)
    {
        property = property.IsNotNull(nameof(property));

        var typeName = property.TypeName.ToString();
        var propertyName = property.Name.ToString();
        if (typeName.StartsWithAny(StringComparison.InvariantCulture, "ModelFramework.Objects.Contracts.I",
                                                                      "ModelFramework.Database.Contracts.I",
                                                                      "ModelFramework.Common.Contracts.I"))
        {
            var isClass = typeName == typeof(IClass).FullName!;
            var defaultCustomBuilderMethodParameterExpression = property.IsNullable || AddNullChecks
                ? "{0}?.ToEntity()"
                : "{0}{2}.ToEntity()";
            var argumentType = typeName.Replace("Contracts.I", "Models.", StringComparison.InvariantCulture) + "Model";
            property.WithCustomBuilderConstructorInitializeExpressionSingleProperty(argumentType);
            property.WithCustomBuilderArgumentTypeSingleProperty(argumentType);
            property.WithCustomBuilderMethodParameterExpression(isClass
                ? "{0}{2}.ToTypedEntity()"
                : defaultCustomBuilderMethodParameterExpression);
        }
        else if (typeName.Contains("Collection<ModelFramework.", StringComparison.InvariantCulture))
        {
            var isCodeStatement = typeName.Contains(typeof(ICodeStatement).FullName!)
                || typeName.Contains(typeof(ISqlStatement).FullName!);
            var isClass = typeName == "System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IClass>";
            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                false,
                typeof(ReadOnlyValueCollection<>).WithoutGenerics(),
                isCodeStatement
                    ? typeName.ReplaceSuffix(">", "Model>", StringComparison.InvariantCulture)
                    : typeName.Replace("Contracts.I", "Models.", StringComparison.InvariantCulture).ReplaceSuffix(">", "Model>", StringComparison.InvariantCulture),
                isCodeStatement
                    ? "{4}{0}.AddRange(source.{0}.Select(x => x.CreateModel()))"
                    : null,
                customBuilderMethodParameterExpression: isClass
                    ? "{0}.Select(x => x.ToTypedEntity())"
                    : "{0}.Select(x => x.ToEntity())"
            );
        }
        else if (typeName.Contains($"Collection<{typeof(string).FullName}", StringComparison.InvariantCulture))
        {
            property.AddMetadata(Objects.MetadataNames.CustomBuilderMethodParameterExpression, $"new {typeof(List<string>).FullName.FixTypeName()}({{0}})");
        }
        else if ((typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName()) && propertyName.In(nameof(IClassProperty.HasGetter), nameof(IClassProperty.HasSetter)))
        {
            property.SetDefaultValueForBuilderClassConstructor(new Literal("true"));
        }
    }
}
