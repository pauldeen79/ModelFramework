using System;

namespace ModelFramework.CodeGeneration
{
    public interface ICodeGenerationProvider
    {
        bool GenerateMultipleFiles { get; }
        string BasePath { get; }
        string Path { get; }
        string DefaultFileName { get; }
        bool RecurseOnDeleteGeneratedFiles { get; }
        string LastGeneratedFilesFileName { get; }
        Action? AdditionalActionDelegate { get; }

        void Initialize(bool generateMultipleFiles, string basePath);
        object CreateGenerator();
        object CreateModel();
        object CreateAdditionalParameters();
    }
}
