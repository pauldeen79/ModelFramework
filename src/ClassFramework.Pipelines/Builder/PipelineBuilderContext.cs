namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderContext
{
    public PipelineBuilderContext(TypeBase sourceModel, PipelineBuilderSettings settings, IFormatProvider formatProvider)
    {
        SourceModel = sourceModel.IsNotNull(nameof(SourceModel));
        Settings = settings.IsNotNull(nameof(settings));
        FormatProvider = formatProvider.IsNotNull(nameof(formatProvider));
    }

    public TypeBase SourceModel { get; }
    public PipelineBuilderSettings Settings { get; }
    public IFormatProvider FormatProvider { get; }
}
