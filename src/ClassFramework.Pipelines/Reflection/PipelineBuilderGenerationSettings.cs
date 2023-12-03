namespace ClassFramework.Pipelines.Reflection;

public record PipelineBuilderGenerationSettings
{
    public bool AllowGenerationWithoutProperties { get; }
    
    public PipelineBuilderGenerationSettings(bool allowGenerationWithoutProperties = false)
    {
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
    }
}
