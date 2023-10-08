namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderContext : BuilderContextBase
{
    public PipelineBuilderContext(BuilderContextBase original) : base(original)
    {
    }

    public PipelineBuilderContext(TypeBase sourceModel, PipelineBuilderSettings settings, IFormatProvider formatProvider) : base(sourceModel, settings, formatProvider)
    {
    }
}
