namespace ClassFramework.TemplateFramework.Extensions;

public static class TemplateEngineExtensions
{
    public static void RenderCsharpChildTemplates(this ITemplateEngine instance, IEnumerable childModels, IGenerationEnvironment generationEnvironment, ITemplateContext context)
        => instance.RenderChildTemplates(
            childModels,
            generationEnvironment,
            context,
            model => new TemplateByModelIdentifier(model!.GetType().GetProperty(nameof(CsharpClassGeneratorViewModel<Domain.Attribute>.Data))!.GetValue(model)));
}
