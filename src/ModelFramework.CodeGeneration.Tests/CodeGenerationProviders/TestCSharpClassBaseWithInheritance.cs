namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public abstract partial class TestCSharpClassBaseWithInheritance : ModelFrameworkCSharpClassBase
{
    protected override bool AddNullChecks => true; // this enables null checks in c'tor of both records and builders

    protected override void FixImmutableClassProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        => FixImmutableBuilderProperties(typeBaseBuilder);

    protected override void FixImmutableBuilderProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
    {
        if (typeBaseBuilder == null)
        {
            // Not possible, but needs to be added because of .net standard 2.0
            return;
        }

        typeBaseBuilder.Properties.ForEach(FixImmutableBuilderProperty);
    }

    protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
    {
        if (instance == null)
        {
            // Not possible, but needs to be added because of .net standard 2.0
            return string.Empty;
        }

        if (forCreate)
        {
            // For creation, the typename doesn't have to be altered/formatted.
            return string.Empty;
        }

        if (instance.Namespace == "ModelFramework.Common.Tests.Test")
        {
            return "ModelFramework.Common.Tests.Test.Contracts.I" + instance.Name;
        }

        return string.Empty;
    }

    private void FixImmutableBuilderProperty(ClassPropertyBuilder property)
    {
        var typeName = property.TypeName.ToString();
        if (typeName.StartsWith("ModelFramework.Common.Tests.Test.Contracts.I", StringComparison.InvariantCulture))
        {
            property.ConvertSinglePropertyToBuilderOnBuilder
            (
                argumentType: typeName.Replace("Test.Contracts.I", "Test.Builders.", StringComparison.InvariantCulture) + "Builder", // replace interface with concrete type
                useLazyInitialization: UseLazyInitialization,
                useTargetTypeNewExpressions: UseTargetTypeNewExpressions
            );
        }
        else if (typeName.Contains("Collection<ModelFramework.", StringComparison.InvariantCulture))
        {
            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                addNullChecks: false, // already checked in constructor by using the AddNulLChecks property, see above in this class
                argumentValidationType: ValidateArgumentsInConstructor,
                typeof(ReadOnlyValueCollection<>).WithoutGenerics(),
                argumentType: typeName.Replace("Test.Contracts.I", "Test.Builders.", StringComparison.InvariantCulture).Replace(">", "Builder>", StringComparison.InvariantCulture) // replace interface with concrete type
            );
        }
    }
}
