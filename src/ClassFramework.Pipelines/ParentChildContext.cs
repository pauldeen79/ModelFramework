namespace ClassFramework.Pipelines;

public record ParentChildContext<TParentContext, TChild>
{
    public ParentChildContext(TParentContext parentContext, TChild childContext, IPipelineGenerationSettings settings)
    {
        ParentContext = parentContext.IsNotNull(nameof(parentContext));
        ChildContext = childContext.IsNotNull(nameof(childContext));
        Settings = settings.IsNotNull(nameof(settings));
    }

    public TParentContext ParentContext { get; }
    public TChild ChildContext { get; }
    public IPipelineGenerationSettings Settings { get; }
}
