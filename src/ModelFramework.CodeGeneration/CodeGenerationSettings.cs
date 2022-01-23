namespace ModelFramework.CodeGeneration
{
    public record CodeGenerationSettings
    {
        public CodeGenerationSettings(string basePath, bool generateMultipleFiles, bool dryRun)
        {
            BasePath = basePath;
            GenerateMultipleFiles = generateMultipleFiles;
            DryRun = dryRun;
        }

        public string BasePath { get; }
        public bool GenerateMultipleFiles { get; }
        public bool DryRun { get; }
    }
}
