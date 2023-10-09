namespace ClassFramework.Pipelines;

public record ParentChildContext<TChild>
{
    public ParentChildContext(PipelineContext<ClassBuilder, BuilderContext> parentContext, TChild childContext)
    {
        ParentContext = parentContext.IsNotNull(nameof(parentContext));
        ChildContext = childContext.IsNotNull(nameof(parentContext));
    }

    public PipelineContext<ClassBuilder, BuilderContext> ParentContext { get; }
    public TChild ChildContext { get; }
}
