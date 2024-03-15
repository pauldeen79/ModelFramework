namespace DatabaseFramework.TemplateFramework;

public abstract class DatabaseSchemaGeneratorBase<TModel> : TemplateBase, IModelContainer<TModel>
{
    protected override void OnSetContext(ITemplateContext value)
    {
        if (Model is ITemplateContextContainer container)
        {
            // Copy Context from template context to ViewModel
            container.Context = value;
        }
    }

    public TModel? Model { get; set; }

    protected void RenderChildTemplateByModel(object model, IGenerationEnvironment generationEnvironment)
    {
        Guard.IsNotNull(Context);
        Context.Engine.RenderChildTemplate(model, generationEnvironment, Context, new TemplateByModelIdentifier(model));
    }

    protected void RenderChildTemplatesByModel(IEnumerable models, StringBuilder builder)
    {
        RenderChildTemplatesByModel(models, new StringBuilderEnvironment(builder));
    }

    protected void RenderChildTemplatesByModel(IEnumerable models, IGenerationEnvironment generationEnvironment)
    {
        Guard.IsNotNull(Context);
        Context.Engine.RenderChildTemplates(models, generationEnvironment, Context, model => new TemplateByModelIdentifier(model));
    }
}
