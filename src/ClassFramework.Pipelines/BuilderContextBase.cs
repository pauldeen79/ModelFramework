namespace ClassFramework.Pipelines;

public abstract record BuilderContextBase
{
    protected BuilderContextBase(TypeBase sourceModel, PipelineBuilderSettings settings, IFormatProvider formatProvider)
    {
        SourceModel = sourceModel.IsNotNull(nameof(SourceModel));
        Settings = settings.IsNotNull(nameof(settings));
        FormatProvider = formatProvider.IsNotNull(nameof(formatProvider));
    }

    public TypeBase SourceModel { get; }
    public PipelineBuilderSettings Settings { get; }
    public IFormatProvider FormatProvider { get; }
}
