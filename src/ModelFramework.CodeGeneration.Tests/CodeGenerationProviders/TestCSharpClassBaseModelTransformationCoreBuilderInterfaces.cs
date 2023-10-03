namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationCoreBuilderInterfaces : TestCSharpClassBaseModelTransformationBase
{
    public override object CreateModel()
        => CreateBuilderInterfaces(GetCoreModelTransformationTypes()
            .Select(x => x.ToInterfaceBuilder().Chain(y => y.Name = "I" + y.Name)));
}
