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

        if (Model is not null && Model.Settings is null)
        {
            var settings = value.GetCsharpClassGeneratorSettings();
            if (settings is not null)
            {
                Model.Settings = settings;
            }
            else
            {
                throw new InvalidOperationException("Could not get Settings from context");
            }
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
