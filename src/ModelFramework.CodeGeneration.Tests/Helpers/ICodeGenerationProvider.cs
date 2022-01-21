namespace ModelFramework.CodeGeneration.Tests.Helpers
{
    public interface ICodeGenerationProvider
    {
        bool GenerateMultipleFiles { get; set; }
        string BasePath { get; set; }
        string Prefix { get; }
        string DefaultFileName { get; }

        object CreateModel();
        object CreateAdditionalParameters();
        object CreateGenerator();
    }
}
