namespace ClassFramework.Pipelines.Entity;

public record EntityContext
{
    public EntityContext(TypeBase sourceModel, PipelineBuilderSettings settings, IFormatProvider formatProvider)
    {
        SourceModel = sourceModel.IsNotNull(nameof(sourceModel));
        Settings = settings.IsNotNull(nameof(settings));
        FormatProvider = formatProvider.IsNotNull(nameof(formatProvider));
    }

    public TypeBase SourceModel { get; }
    public PipelineBuilderSettings Settings { get; }
    public IFormatProvider FormatProvider { get; }
}
