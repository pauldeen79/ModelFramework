namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderGenerationSettings : IPipelineBuilderGenerationSettings
{
    public bool AddSetters { get; }
    public Visibility? SetterVisibility { get; }
    public bool EnableNullableReferenceTypes { get; }
    public bool AddNullChecks { get; }
    public bool AllowGenerationWithoutProperties { get; }

    public PipelineBuilderGenerationSettings(
        bool addSetters = false,
        Visibility? setterVisibility = null,
        bool addNullChecks = false,
        bool enableNullableReferenceTypes = false,
        bool allowGenerationWithoutProperties = false)
    {
        AddSetters = addSetters;
        SetterVisibility = setterVisibility;
        AddNullChecks = addNullChecks;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
    }
}
