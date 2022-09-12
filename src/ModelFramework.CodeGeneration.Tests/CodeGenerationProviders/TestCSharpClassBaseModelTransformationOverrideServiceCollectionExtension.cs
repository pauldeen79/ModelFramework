namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationOverrideServiceCollectionExtension : TestCSharpClassBaseModelTransformationBase
{
    public override object CreateModel()
        => CreateServiceCollectionExtensions(
            "MyNamespace.Domain.Extensions",
            "ServiceCollectionExtensions",
            "AddHandlers",
            GetOverrideModelTransformationTypes(),
            x => $".AddSingleton<IHandler, {x.Name}Handler>()");
}
