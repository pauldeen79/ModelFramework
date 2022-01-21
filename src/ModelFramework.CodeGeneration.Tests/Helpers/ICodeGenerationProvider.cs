namespace ModelFramework.CodeGeneration.Tests.Helpers
{
    public interface ICodeGenerationProvider
    {
        object CreateModel();
        object CreateAdditionalParameters();
        object CreateGenerator();
    }
}
