namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public partial class TestCSharpClassBase
{
    protected TestCSharpClassBase() { }

    public static ITypeBase[] CreateTestModels() => GetTestModels();
}
