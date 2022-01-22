using System.Text;
using TextTemplateTransformationFramework.Runtime;

namespace ModelFramework.CodeGeneration
{
    public class GenerateCode
    {
        private const string Parent = "\\";

        private GenerateCode(string basePath)
        {
            GenerationEnvironment = new StringBuilder();
            TemplateFileManager = new TemplateFileManager(b => GenerationEnvironment = b, () => GenerationEnvironment, basePath);
        }

        public TemplateFileManager TemplateFileManager { get; }
        public StringBuilder GenerationEnvironment { get; private set; }

        public static GenerateCode For<T>(CodeGenerationSettings settings)
            where T : ICodeGenerationProvider, new()
        {
            var provider = new T();
            provider.BasePath = settings.BasePath;
            provider.GenerateMultipleFiles = settings.GenerateMultipleFiles;
            var result = new GenerateCode(provider.BasePath);
            var generator = provider.CreateGenerator();
            var shouldSave = !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun;
            var shouldUseLastGeneratedFiles = !string.IsNullOrEmpty(settings.LastGeneratedFilesFileName);
            var additionalParameters = provider.CreateAdditionalParameters();

            //TODO: Add model parameter to render template (default value null)
            if (!provider.GenerateMultipleFiles)
            {
                TemplateRenderHelper.RenderTemplate(template: generator,
                                                    generationEnvironment: result.TemplateFileManager.StartNewFile(provider.Prefix + Parent + provider.DefaultFileName),
                                                    additionalParameters: additionalParameters);
            }
            else
            {
                TemplateRenderHelper.RenderTemplate(template: generator,
                                                    generationEnvironment: result.TemplateFileManager,
                                                    fileNamePrefix: provider.Prefix + "\\",
                                                    defaultFileName: provider.DefaultFileName,
                                                    additionalParameters: additionalParameters);
            }

            result.TemplateFileManager.Process(true, shouldSave);

            if (shouldSave)
            {
                if (shouldUseLastGeneratedFiles)
                {
                    var prefixedLastGeneratedFilesFileName = provider.Prefix + Parent + (settings.GenerateMultipleFiles ? settings.LastGeneratedFilesFileName : provider.DefaultFileName);
                    //TODO: Add Recurse option to delete last generated files (default value true)
                    result.TemplateFileManager.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFileName);
                    result.TemplateFileManager.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFileName);
                }
                result.TemplateFileManager.SaveAll();
            }
            return result;
        }
    }
}
