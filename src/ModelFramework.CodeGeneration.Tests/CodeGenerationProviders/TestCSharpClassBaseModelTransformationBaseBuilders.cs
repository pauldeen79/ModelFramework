namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationBaseBuilders : TestCSharpClassBaseModelTransformationBase
{
    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetAbstractModelTransformationTypes(),
            "MyNamespace.Domain",
            "MyNamespace.Domain.Builders");
}
