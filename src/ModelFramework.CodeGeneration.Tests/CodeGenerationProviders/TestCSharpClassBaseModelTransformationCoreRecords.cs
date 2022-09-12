namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationCoreRecords : TestCSharpClassBaseModelTransformationBase
{
    public override object CreateModel()
        => GetImmutableClasses(
            GetCoreModelTransformationTypes(),
            "MyNamespace.Domain");
}
