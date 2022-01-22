namespace ModelFramework.CodeGeneration
{
    public interface ICodeGenerationProvider
    {
        bool GenerateMultipleFiles { get; set; }
        string BasePath { get; set; }
        string Prefix { get; }
        string DefaultFileName { get; }

        object CreateAdditionalParameters();
        object CreateGenerator();
    }
}
