namespace ModelFramework.CodeGeneration
{
    public interface ICodeGenerationProvider
    {
        bool GenerateMultipleFiles { get; set; }
        string BasePath { get; set; }
        string Path { get; }
        string DefaultFileName { get; }
        bool RecurseOnDeleteGeneratedFiles { get; }
        string LastGeneratedFilesFileName { get; }

        object CreateGenerator();
        object CreateModel();
        object CreateAdditionalParameters();
    }
}
