namespace ClassFramework.Pipelines.Reflection;

public record PipelineBuilderGenerationSettings
{
    public bool AllowGenerationWithoutProperties { get; }
    public bool UseBaseClassFromSourceModel { get; }
    public Func<System.Attribute, AttributeBuilder?>? InitializeDelegate { get; }

    public PipelineBuilderGenerationSettings(
        bool allowGenerationWithoutProperties = false,
        bool useBaseClassFromSourceModel = true,
        Func<System.Attribute, AttributeBuilder?>? initializeDelegate = null)
    {
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        UseBaseClassFromSourceModel = useBaseClassFromSourceModel;
        InitializeDelegate = initializeDelegate;
    }
}
