namespace ClassFramework.Pipelines;

public record ParentChildContext<TModel, TParent, TChild>
{
    public ParentChildContext(PipelineContext<TModel, TParent> parentContext, TChild childContext, IPipelineGenerationSettings settings)
    {
        ParentContext = parentContext.IsNotNull(nameof(parentContext));
        ChildContext = childContext.IsNotNull(nameof(childContext));
        Settings = settings.IsNotNull(nameof(settings));
    }

    public PipelineContext<TModel, TParent> ParentContext { get; }
    public TChild ChildContext { get; }
    public IPipelineGenerationSettings Settings { get; }
}
