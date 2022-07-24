namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public abstract partial class TestCSharpClassBase : ModelFrameworkCSharpClassBase
{
    protected override bool InheritFromInterfaces => false;

    protected override void FixImmutableClassProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        => FixImmutableBuilderProperties(typeBaseBuilder);

    protected override void FixImmutableBuilderProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
    {
        if (typeBaseBuilder == null)
        {
            // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
            return;
        }

        foreach (var property in typeBaseBuilder.Properties)
        {
            FixImmutableBuilderProperty(property);
        }
    }

    private static void FixImmutableBuilderProperty(ClassPropertyBuilder property)
    {
        var typeName = property.TypeName.FixTypeName();
        if (typeName.StartsWith("ModelFramework.Common.Contracts.Test.I", StringComparison.InvariantCulture))
        {
            property.TypeName = typeName.Replace("Contracts.Test.I", "Test.", StringComparison.InvariantCulture);
        }
        else if (typeName.Contains("Collection<ModelFramework.", StringComparison.InvariantCulture))
        {
            property.TypeName = typeName.Replace("Contracts.Test.I", "Test.", StringComparison.InvariantCulture);
        }
    }
}
