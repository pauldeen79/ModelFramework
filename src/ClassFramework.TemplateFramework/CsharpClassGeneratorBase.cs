namespace ClassFramework.TemplateFramework;

public abstract class CsharpClassGeneratorBase<TModel> : TemplateBase, IModelContainer<TModel>
{
    protected CsharpClassGeneratorBase(IViewModelFactory viewModelFactory) : base(viewModelFactory)
    {
    }

    protected override void OnSetContext(ITemplateContext value)
    {
        if (Model is ITemplateContextContainer container)
        {
            container.Context = value;
        }
    }

    public TModel? Model { get; set; }
}
