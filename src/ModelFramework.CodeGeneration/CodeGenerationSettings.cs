namespace ModelFramework.CodeGeneration
{
    public record CodeGenerationSettings
    {
        public CodeGenerationSettings(string basePath, bool generateMultipleFiles, bool dryRun, string lastGeneratedFilesFileName)
        {
            BasePath = basePath;
            GenerateMultipleFiles = generateMultipleFiles;
            DryRun = dryRun;
            LastGeneratedFilesFileName = lastGeneratedFilesFileName;
        }

        public string BasePath { get; }
        public bool GenerateMultipleFiles { get; }
        public bool DryRun { get; }
        public string LastGeneratedFilesFileName { get; }
    }
}
