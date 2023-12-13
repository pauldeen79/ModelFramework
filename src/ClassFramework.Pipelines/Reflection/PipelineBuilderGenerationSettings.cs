namespace ClassFramework.Pipelines.Reflection;

public record PipelineBuilderGenerationSettings
{
    public bool AllowGenerationWithoutProperties { get; }
    public Func<System.Attribute, AttributeBuilder?>? InitializeDelegate { get; }

    public PipelineBuilderGenerationSettings(bool allowGenerationWithoutProperties = false, Func<System.Attribute, AttributeBuilder?>? initializeDelegate = null)
    {
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        InitializeDelegate = initializeDelegate;
    }
}
