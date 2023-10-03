namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationOverrideBuilderFactoryCustomCode : TestCSharpClassBaseModelTransformationBase
{
    public override object CreateModel()
        => CreateBuilderFactories(
            GetOverrideModelTransformationTypes(),
            new("MyNamespace.Domain.Builders",
            "MyClassBuilderFactory",
            "MyNamespace.Domain.MyClass",
            "MyNamespace.Domain.Builders.MyClass",
            "MyClassBuilder",
            "MyNamespace.Domain"),
            "// custom code goes here");
}
