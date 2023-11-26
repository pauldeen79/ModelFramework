namespace ClassFramework.TemplateFramework;

public abstract class CsharpClassGeneratorBase<TModel> : IModelContainer<TModel>, ITemplateContextContainer
{
    public ITemplateContext Context { get; set; } = default!;
    public TModel? Model { get; set; }
}
