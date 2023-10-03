namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationCoreInterfaces : TestCSharpClassBaseModelTransformationBase
{
    public override object CreateModel()
        => GetCoreModelTransformationTypes()
            .Select(x => x.ToInterfaceBuilder().Chain(y => y.Name = "I" + y.Name))
            .Select(CreateInterface)
            .ToArray();
}
