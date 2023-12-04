namespace ClassFramework.TemplateFramework.Templates;

public abstract class TemplateBase : ITemplateContextContainer
{
    private ITemplateContext _context = default!;
    public ITemplateContext Context
    {
        get
        {
            return _context;
        }
        set
        {
            _context = value;
            OnSetContext(value!);
        }
    }

    protected virtual void OnSetContext(ITemplateContext value)
    {
    }

    protected void RenderChildTemplateByModel(object model, StringBuilder builder, CsharpClassGeneratorSettings settings)
    {
        RenderChildTemplateByModel(model, new StringBuilderEnvironment(builder), settings);
    }

    protected void RenderChildTemplateByModel(object model, IGenerationEnvironment generationEnvironment, CsharpClassGeneratorSettings settings)
    {
        Context.Engine.RenderChildTemplate(model, generationEnvironment, Context, new ViewModelTemplateByModelIdentifier(model, settings));
    }

    protected void RenderChildTemplatesByModel(IEnumerable models, StringBuilder builder, CsharpClassGeneratorSettings settings)
    {
        RenderChildTemplatesByModel(models, new StringBuilderEnvironment(builder), settings);
    }

    protected void RenderChildTemplatesByModel(IEnumerable models, IGenerationEnvironment generationEnvironment, CsharpClassGeneratorSettings settings)
    {
        Context.Engine.RenderChildTemplates(models, generationEnvironment, Context, model => new ViewModelTemplateByModelIdentifier(model, settings));
    }
}
