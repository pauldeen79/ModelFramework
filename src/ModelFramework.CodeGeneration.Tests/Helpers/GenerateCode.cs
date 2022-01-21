using TextTemplateTransformationFramework.Runtime;

namespace ModelFramework.CodeGeneration.Tests.Helpers
{
    public static class GenerateCode
    {
        public static string For<T>()
            where T : ICodeGenerationProvider, new()
        {
            var provider = new T();
            return TemplateRenderHelper.GetTemplateOutput(provider.CreateGenerator(),
                                                          provider.CreateModel(),
                                                          additionalParameters: provider.CreateAdditionalParameters());
        }
    }
}
