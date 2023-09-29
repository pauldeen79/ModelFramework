namespace ClassFramework.Pipelines;

public record BuilderPipelineBuilderSettings
{
    public BuilderPipelineBuilderSettings(BuilderPipelineBuilderNameSettings? nameSettings = null)
    {
        NameSettings = nameSettings ?? new();
    }

    public BuilderPipelineBuilderNameSettings NameSettings { get; }
}
