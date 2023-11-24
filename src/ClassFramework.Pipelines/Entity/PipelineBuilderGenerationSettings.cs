namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderGenerationSettings
{
    public bool AddSetters { get; }
    public bool AddBackingFields { get; }
    public Visibility? SetterVisibility { get; }
    public bool AllowGenerationWithoutProperties { get; }
    public bool CreateRecord { get; }

    public PipelineBuilderGenerationSettings(
        bool addSetters = false,
        Visibility? setterVisibility = null,
        bool allowGenerationWithoutProperties = false,
        bool createRecord = false,
        bool addBackingFields = false)
    {
        AddSetters = addSetters;
        SetterVisibility = setterVisibility;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        CreateRecord = createRecord;
        AddBackingFields = addBackingFields;
    }
}
