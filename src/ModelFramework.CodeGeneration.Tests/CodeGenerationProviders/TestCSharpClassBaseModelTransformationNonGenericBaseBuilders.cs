namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationNonGenericBaseBuilders : TestCSharpClassBaseModelTransformationBase
{
    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override string FileNameSuffix => ".nongeneric.template.generated";

    public override object CreateModel()
        => GetImmutableNonGenericBuilderClasses(
            GetAbstractModelTransformationTypes(),
            "MyNamespace.Domain",
            "MyNamespace.Domain.Builders");
}
