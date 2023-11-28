namespace ClassFramework.TemplateFramework.Extensions;

public static class TemplateEngineExtensions
{
    public static void RenderCsharpChildTemplate(this ITemplateEngine instance, object? childModel, IGenerationEnvironment generationEnvironment, ITemplateContext context)
        => instance.RenderChildTemplate(childModel, generationEnvironment, context, new TemplateByModelIdentifier(childModel));

    public static void RenderCsharpChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context)
        => instance.RenderChildTemplates(childModels, generationEnvironment, context, model => new TemplateByModelIdentifier(model));
}
