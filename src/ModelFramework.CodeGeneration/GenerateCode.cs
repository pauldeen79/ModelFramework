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
            var provider = new T
            {
                BasePath = settings.BasePath,
                GenerateMultipleFiles = settings.GenerateMultipleFiles
            };
            var result = new GenerateCode(provider.BasePath);
            var generator = provider.CreateGenerator();
            var shouldSave = !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun;
            var shouldUseLastGeneratedFiles = !string.IsNullOrEmpty(provider.LastGeneratedFilesFileName);
            var additionalParameters = provider.CreateAdditionalParameters();

            TemplateRenderHelper.RenderTemplateWithModel(template: generator,
                                                         generationEnvironment: !provider.GenerateMultipleFiles
                                                            ? result.TemplateFileManager.StartNewFile(provider.Path + Parent + provider.DefaultFileName)
                                                            : result.TemplateFileManager,
                                                         model: provider.CreateModel(),
                                                         defaultFileName: !provider.GenerateMultipleFiles
                                                            ? null
                                                            : provider.DefaultFileName,
                                                         additionalActionDelegate: provider.AdditionalActionDelegate,
                                                         additionalParameters: additionalParameters);

            result.TemplateFileManager.Process(true, shouldSave);

            if (shouldSave)
            {
                if (shouldUseLastGeneratedFiles)
                {
                    var prefixedLastGeneratedFilesFileName = provider.Path + Parent + (settings.GenerateMultipleFiles ? provider.LastGeneratedFilesFileName : provider.DefaultFileName);
                    result.TemplateFileManager.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFileName, provider.RecurseOnDeleteGeneratedFiles);
                    result.TemplateFileManager.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFileName);
                }
                result.TemplateFileManager.SaveAll();
            }
            return result;
        }
    }
}
