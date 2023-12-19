namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationOverrideBuilderFactory : TestCSharpClassBaseModelTransformationBase
{
    public override object CreateModel()
        => CreateBuilderFactories(
            GetOverrideModelTransformationTypes(),
            new("MyNamespace.Domain.Builders",
            "MyClassBuilderFactory",
            "MyNamespace.Domain.MyClass",
            "MyNamespace.Domain.Builders.MyClass",
            "MyClassBuilder",
            "MyNamespace.Domain"));
}
