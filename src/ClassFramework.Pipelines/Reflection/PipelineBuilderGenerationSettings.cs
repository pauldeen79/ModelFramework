namespace ClassFramework.Pipelines.Reflection;

public record PipelineBuilderGenerationSettings
{
    public bool AllowGenerationWithoutProperties { get; }
    public bool UseBaseClassFromSourceModel { get; }
    public bool Partial { get; }
    public Func<System.Attribute, AttributeBuilder?>? InitializeDelegate { get; }

    public PipelineBuilderGenerationSettings(
        bool allowGenerationWithoutProperties = false,
        bool useBaseClassFromSourceModel = true,
        bool partial = true,
        Func<System.Attribute, AttributeBuilder?>? initializeDelegate = null)
    {
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        UseBaseClassFromSourceModel = useBaseClassFromSourceModel;
        Partial = partial;
        InitializeDelegate = initializeDelegate;
    }
}
