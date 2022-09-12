namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationBaseRecords : TestCSharpClassBaseModelTransformationBase
{
    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;

    public override object CreateModel()
        => GetImmutableClasses(
            GetAbstractModelTransformationTypes(),
            "MyNamespace.Domain");
}
