﻿namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public abstract partial class TestCSharpClassBase : ModelFrameworkCSharpClassBase
{
    protected override bool InheritFromInterfaces => false;
    protected override bool AddNullChecks => true; // this enables null checks in c'tor of both records and builders

    protected override void FixImmutableClassProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        => FixImmutableBuilderProperties(typeBaseBuilder);

    protected override void FixImmutableBuilderProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
    {
        if (typeBaseBuilder == null)
        {
            // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
            return;
        }

        typeBaseBuilder.Properties.ForEach(FixImmutableBuilderProperty);
    }

    private void FixImmutableBuilderProperty(ClassPropertyBuilder property)
    {
        var typeName = property.TypeName.FixTypeName();
        if (typeName.StartsWith("ModelFramework.Common.Tests.Test.Contracts.I", StringComparison.InvariantCulture))
        {
            property.TypeName = typeName.Replace("Test.Contracts.I", "Test.", StringComparison.InvariantCulture);

            property.ConvertSinglePropertyToBuilderOnBuilder
            (
                typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture) + "Builder",
                useLazyInitialization: UseLazyInitialization,
                useTargetTypeNewExpressions: UseTargetTypeNewExpressions
            );
        }
        else if (typeName.Contains("Collection<ModelFramework.", StringComparison.InvariantCulture))
        {
            property.TypeName = typeName.Replace("Test.Contracts.I", "Test.", StringComparison.InvariantCulture);

            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                addNullChecks: false, // already checked in constructor by using the AddNulLChecks property, see above in this class
                typeof(ReadOnlyValueCollection<>).WithoutGenerics(),
                typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
            );
        }
    }
}
