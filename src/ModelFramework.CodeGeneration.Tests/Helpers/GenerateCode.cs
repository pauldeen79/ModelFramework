using System;
using System.Collections.Generic;
using System.Text;
using TextTemplateTransformationFramework.Runtime;

namespace ModelFramework.CodeGeneration.Tests.Helpers
{
    public class GenerateCode
    {
        private GenerateCode(string basePath)
        {
            GenerationEnvironment = new StringBuilder();
            TemplateFileManager = new TemplateFileManager(b => GenerationEnvironment = b, () => GenerationEnvironment, basePath);
        }

        public TemplateFileManager TemplateFileManager { get; private set; }
        public StringBuilder GenerationEnvironment { get; private set; }

        public static GenerateCode For<T>(string basePath, bool generateMultipleFiles, bool dryRun, string lastGeneratedFilesFileName)
            where T : ICodeGenerationProvider, new()
        {
            var provider = new T();
            provider.BasePath = basePath;
            provider.GenerateMultipleFiles = generateMultipleFiles;
            var result = new GenerateCode(provider.BasePath);
            var generator = provider.CreateGenerator();
            var shouldSave = !string.IsNullOrEmpty(provider.BasePath) && !dryRun;
            var shouldUseLastGeneratedFiles = !string.IsNullOrEmpty(lastGeneratedFilesFileName);
            //var session = new Dictionary<string, object>()
            //{
            //    { "ShouldSave", shouldSave },
            //    { "ShouldUseLastGeneratedFiles", shouldUseLastGeneratedFiles }
            //};
            var additionalParameters = provider.CreateAdditionalParameters();

            if (!provider.GenerateMultipleFiles)
            {
                TemplateRenderHelper.RenderTemplate(template: generator,
                                                    generationEnvironment: result.TemplateFileManager.StartNewFile(provider.Prefix + "\\" + provider.DefaultFileName),
                                                    //session: session,
                                                    additionalParameters: additionalParameters);
            }
            else
            {
                TemplateRenderHelper.RenderTemplate(template: generator,
                                                    generationEnvironment: result.TemplateFileManager,
                                                    //session: session,
                                                    fileNamePrefix: provider.Prefix + "\\",
                                                    defaultFileName: provider.DefaultFileName,
                                                    additionalParameters: additionalParameters);
            }

            result.TemplateFileManager.Process(true, shouldSave);

            if (shouldSave)
            {
                if (shouldUseLastGeneratedFiles)
                {
                    result.TemplateFileManager.DeleteLastGeneratedFiles(lastGeneratedFilesFileName);
                    result.TemplateFileManager.SaveLastGeneratedFiles(lastGeneratedFilesFileName);
                }
                result.TemplateFileManager.SaveAll();
            }
            return result;
        }
    }
}
