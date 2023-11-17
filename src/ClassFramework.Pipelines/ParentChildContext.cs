namespace ClassFramework.Pipelines;

public record ParentChildContext<TParent, TChild>
{
    public ParentChildContext(PipelineContext<ClassBuilder, TParent> parentContext, TChild childContext, IPipelineGenerationSettings settings)
    {
        ParentContext = parentContext.IsNotNull(nameof(parentContext));
        ChildContext = childContext.IsNotNull(nameof(childContext));
        Settings = settings.IsNotNull(nameof(settings));
    }

    public PipelineContext<ClassBuilder, TParent> ParentContext { get; }
    public TChild ChildContext { get; }
    public IPipelineGenerationSettings Settings { get; }
}
