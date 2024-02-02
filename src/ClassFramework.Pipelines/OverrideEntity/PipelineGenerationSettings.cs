namespace ClassFramework.Pipelines.OverrideEntity;

public record PipelineGenerationSettings
{
    public bool CreateRecord { get; }
    public bool AllowGenerationWithoutProperties { get; }

    public PipelineGenerationSettings(
        bool createRecord = false,
        bool allowGenerationWithoutProperties = false)
    {
        CreateRecord = createRecord;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
    }
}
