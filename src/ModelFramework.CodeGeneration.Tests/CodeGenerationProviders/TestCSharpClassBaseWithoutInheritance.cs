﻿namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public abstract partial class TestCSharpClassBaseWithoutInheritance : ModelFrameworkCSharpClassBase
{
    protected override bool InheritFromInterfaces => false;
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

    private void FixImmutableBuilderProperty(ClassPropertyBuilder property)
    {
        var typeName = property.TypeName;
        if (typeName.StartsWith("ModelFramework.Common.Tests.Test.Contracts.I", StringComparison.InvariantCulture))
        {
            property.WithTypeName(typeName.Replace("Test.Contracts.I", "Test.", StringComparison.InvariantCulture));

            property.ConvertSinglePropertyToBuilderOnBuilder
            (
                argumentType: null, // using builders namespace instead
                useLazyInitialization: UseLazyInitialization,
                useTargetTypeNewExpressions: UseTargetTypeNewExpressions,
                buildersNamespace: "ModelFramework.Common.Tests.Test.Builders"
            );
        }
        else if (typeName.Contains("Collection<ModelFramework.", StringComparison.InvariantCulture))
        {
            property.WithTypeName(typeName.Replace("Test.Contracts.I", "Test.", StringComparison.InvariantCulture));
            property.AddCollectionBackingFieldOnImmutableClass(typeof(ValueCollection<>));
            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                addNullChecks: false, // already checked in constructor by using the AddNullChecks property, see above in this class
                collectionType: typeof(ValueCollection<>).WithoutGenerics(),
                argumentType: null, // using builders namespace instead
                buildersNamespace: "ModelFramework.Common.Tests.Test.Builders",
                builderCollectionTypeName: typeof(IEnumerable<>).WithoutGenerics()
            );
        }
        else if (typeName.IsStringTypeName())
        {
            property.ConvertStringPropertyToStringBuilderPropertyOnBuilder(UseLazyInitialization);
        }
    }
}
