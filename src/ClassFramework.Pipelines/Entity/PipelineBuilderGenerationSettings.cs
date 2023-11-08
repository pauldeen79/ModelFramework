namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderGenerationSettings
{
    public bool AddSetters { get; }
    public Visibility? SetterVisibility { get; }
    public bool EnableNullableReferenceTypes { get; }
    public bool AllowGenerationWithoutProperties { get; }

    public PipelineBuilderGenerationSettings(
        bool addSetters = false,
        Visibility? setterVisibility = null,
        bool enableNullableReferenceTypes = false,
        bool allowGenerationWithoutProperties = false)
    {
        AddSetters = addSetters;
        SetterVisibility = setterVisibility;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
    }
}
