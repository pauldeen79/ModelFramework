namespace ClassFramework.TemplateFramework.Templates;

public abstract class TemplateBase : ITemplateContextContainer, ICsharpClassGeneratorSettingsContainer
{
    private readonly IViewModelFactory _viewModelFactory;

    protected TemplateBase(IViewModelFactory viewModelFactory)
    {
        Guard.IsNotNull(viewModelFactory);

        _viewModelFactory = viewModelFactory;
    }

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
            if (value?.Model is CsharpClassGeneratorViewModelBase vmb)
            {
                // Copy context from generator to ViewModel, so it can be used there
                Settings = vmb.Settings;
            }

            OnSetContext(value!);
        }
    }

    protected virtual void OnSetContext(ITemplateContext value)
    {
    }

    public CsharpClassGeneratorSettings Settings { get; set; } = default!;

    protected void RenderChildTemplateByModel(object model, StringBuilder builder)
    {
        RenderChildTemplateByModel(model, new StringBuilderEnvironment(builder));
    }

    protected void RenderChildTemplateByModel(object model, IGenerationEnvironment generationEnvironment)
    {
        var viewModel = CreateViewModel(model);

        Context.Engine.RenderChildTemplate(viewModel, generationEnvironment, Context, new TemplateByModelIdentifier(viewModel));
    }

    protected void RenderChildTemplatesByModel(IEnumerable models, StringBuilder builder)
    {
        RenderChildTemplatesByModel(models, new StringBuilderEnvironment(builder));
    }

    protected void RenderChildTemplatesByModel(IEnumerable models, IGenerationEnvironment generationEnvironment)
    {
        Context.Engine.RenderChildTemplates(models.OfType<object>().Select(CreateViewModel), generationEnvironment, Context, model => new TemplateByModelIdentifier(model));
    }

    private object CreateViewModel(object model)
    {
        var viewModel = _viewModelFactory.Create(model);

        var modelProperty = viewModel.GetType().GetProperty(nameof(IModelContainer<object>.Model));
        if (modelProperty is not null)
        {
            modelProperty.SetValue(viewModel, model);
        }

        if (viewModel is ITemplateContextContainer contextContainer)
        {
            contextContainer.Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), model));
        }

        if (viewModel is ICsharpClassGeneratorSettingsContainer settingsContainer)
        {
            settingsContainer.Settings = Settings;
        }

        return viewModel;
    }
}
