namespace ClassFramework.Pipelines.Entity;

public record EntityContext : ContextBase<TypeBase, PipelineBuilderSettings>
{
    public EntityContext(TypeBase sourceModel, PipelineBuilderSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }
}
