namespace ClassFramework.Pipelines.Reflection;

public record PipelineBuilderGenerationSettings
{
    public bool AllowGenerationWithoutProperties { get; }
    public bool UseBaseClassFromSourceModel { get; }
    public bool Partial { get; }
    public bool CreateConstructors { get; }
    public Func<System.Attribute, AttributeBuilder> AttributeInitializeDelegate { get; }

    public PipelineBuilderGenerationSettings(
        bool allowGenerationWithoutProperties = false,
        bool useBaseClassFromSourceModel = true,
        bool partial = true,
        bool createConstructors = true,
        Func<System.Attribute, AttributeBuilder>? attributeInitializeDelegate = null)
    {
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        UseBaseClassFromSourceModel = useBaseClassFromSourceModel;
        Partial = partial;
        CreateConstructors = createConstructors;
        AttributeInitializeDelegate = attributeInitializeDelegate ?? DefaultInitializer;
    }

    private static AttributeBuilder DefaultInitializer(System.Attribute sourceAttribute)
        =>  new AttributeBuilder().WithName(sourceAttribute.GetType());
}
