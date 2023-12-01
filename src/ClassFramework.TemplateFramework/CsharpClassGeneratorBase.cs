namespace ClassFramework.TemplateFramework;

public abstract class CsharpClassGeneratorBase<TModel> : IModelContainer<TModel>, ITemplateContextContainer
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
            if (Model is ITemplateContextContainer container)
            {
                container.Context = value;
            }
        }
    }
    public TModel? Model { get; set; }
}
