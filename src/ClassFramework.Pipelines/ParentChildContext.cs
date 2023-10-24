namespace ClassFramework.Pipelines;

public record ParentChildContext<TParent, TChild>
{
    public ParentChildContext(PipelineContext<ClassBuilder, TParent> parentContext, TChild childContext)
    {
        ParentContext = parentContext.IsNotNull(nameof(parentContext));
        ChildContext = childContext.IsNotNull(nameof(parentContext));
    }

    public PipelineContext<ClassBuilder, TParent> ParentContext { get; }
    public TChild ChildContext { get; }
}
