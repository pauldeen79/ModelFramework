namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public abstract partial class TestCSharpClassBaseWithoutInheritance : ModelFrameworkCSharpClassBase
{
    protected override bool InheritFromInterfaces => false;
    protected override bool AddNullChecks => true; // this enables null checks in c'tor of both records and builders

    protected override AttributeBuilder AttributeInitializeDelegate(Attribute sourceAttribute)
    {
        if (sourceAttribute == null)
        {
            // Not possible, but needs to be added because of .net standard 2.0
            return new();
        }

        var result = new AttributeBuilder().WithName(sourceAttribute.GetType().FullName!);
        if (sourceAttribute is StringLengthAttribute sla)
        {
            result.AddParameters(new AttributeParameterBuilder().WithValue(sla.MaximumLength));
            if (sla.MinimumLength > 0)
            {
                result.AddParameters(new AttributeParameterBuilder().WithName(nameof(sla.MinimumLength)).WithValue(sla.MinimumLength));
            }
        }

        return result;
    }

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
        var typeName = property.TypeName.FixTypeName();
        if (typeName.StartsWith("ModelFramework.Common.Tests.Test.Contracts.I", StringComparison.InvariantCulture))
        {
            //TODO: Add new extension method for this, e.g. ReplaceNamespace(string oldNamespace, string newNamespace, bool remove interfacePrefix)
            property.TypeName = typeName.Replace("Test.Contracts.I", "Test.", StringComparison.InvariantCulture);

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
            property.TypeName = typeName.Replace("Test.Contracts.I", "Test.", StringComparison.InvariantCulture);

            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                addNullChecks: false, // already checked in constructor by using the AddNullChecks property, see above in this class
                typeof(ReadOnlyValueCollection<>).WithoutGenerics(),
                argumentType: null, // using builders namespace instead
                buildersNamespace: "ModelFramework.Common.Tests.Test.Builders"
            );
        }
        else if (typeName.IsStringTypeName())
        {
            //TODO: Add new extension method for this, e.g. ConvertSingleStringPropertyToStringBuilderOnBuilder, which also includes setting the default value based on IsNullable (possibly override whether it has to be initialized or not)
            property.ConvertSinglePropertyToBuilderOnBuilder
            (
                argumentType: typeof(System.Text.StringBuilder).FullName,
                //TODO: Get rid of these ugly {0}{1}{2} things, by using named format strings e.g. {PropertyName}, {PropertyNamePascalCased}, {Nullable} which in turn can also be added as constants for type safety
                customBuilderMethodParameterExpression: "{0}?.ToString()",
                customBuilderConstructorInitializeExpression: "_{1}Delegate = new (() => new System.Text.StringBuilder(source.{0}))"
            );
            property.SetDefaultValueForBuilderClassConstructor(/*new Literal("new System.Text.StringBuilder()")*/ new Literal("default"));
        }
    }
}
