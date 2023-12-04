namespace ClassFramework.TemplateFramework;

public abstract class CsharpClassGeneratorBase<TModel> : TemplateBase, IModelContainer<TModel>
    where TModel : ICsharpClassGeneratorSettingsContainer
{
    protected override void OnSetContext(ITemplateContext value)
    {
        if (Model is ITemplateContextContainer container)
        {
            // Copy context from generator to ViewModel, so it can be used there
            container.Context = value;
        }
    }

    public TModel? Model { get; set; }

    protected void RenderChildTemplateByModel(object model, IGenerationEnvironment generationEnvironment)
    {
        Guard.IsNotNull(Context);
        Guard.IsNotNull(Model);
        Context.Engine.RenderChildTemplate(model, generationEnvironment, Context, new ViewModelTemplateByModelIdentifier(model, Model.Settings));
    }

    protected void RenderChildTemplatesByModel(IEnumerable models, StringBuilder builder)
    {
        RenderChildTemplatesByModel(models, new StringBuilderEnvironment(builder));
    }

    protected void RenderChildTemplatesByModel(IEnumerable models, IGenerationEnvironment generationEnvironment)
    {
        Guard.IsNotNull(Context);
        Guard.IsNotNull(Model);
        Context.Engine.RenderChildTemplates(models, generationEnvironment, Context, model => new ViewModelTemplateByModelIdentifier(model, Model.Settings));
    }
}
