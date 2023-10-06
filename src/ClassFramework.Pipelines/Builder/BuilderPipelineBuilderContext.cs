namespace ClassFramework.Pipelines.Builder;

public record BuilderPipelineBuilderContext
{
    public BuilderPipelineBuilderContext(TypeBase sourceModel, BuilderPipelineBuilderSettings settings, IFormatProvider formatProvider)
    {
        SourceModel = sourceModel.IsNotNull(nameof(SourceModel));
        Settings = settings.IsNotNull(nameof(settings));
        FormatProvider = formatProvider.IsNotNull(nameof(formatProvider));
    }

    public TypeBase SourceModel { get; }
    public BuilderPipelineBuilderSettings Settings { get; }
    public IFormatProvider FormatProvider { get; }
}
