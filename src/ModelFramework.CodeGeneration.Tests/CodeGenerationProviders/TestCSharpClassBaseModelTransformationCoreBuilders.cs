namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationCoreBuilders : TestCSharpClassBaseModelTransformationBase
{
    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetCoreModelTransformationTypes(),
            "MyNamespace.Domain",
            "MyNamespace.Domain.Builders");
}
