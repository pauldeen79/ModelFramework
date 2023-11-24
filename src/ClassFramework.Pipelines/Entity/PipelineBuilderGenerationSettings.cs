namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderGenerationSettings
{
    public bool AddSetters { get; }
    public bool AddBackingFields { get; }
    public bool CreateRecord { get; }
    public bool CreateAsObservable { get; }
    public Visibility? SetterVisibility { get; }
    public bool AllowGenerationWithoutProperties { get; }

    public PipelineBuilderGenerationSettings(
        bool addSetters = false,
        bool addBackingFields = false,
        bool createRecord = false,
        bool createAsObservable = false,
        Visibility? setterVisibility = null,
        bool allowGenerationWithoutProperties = false)
    {
        AddSetters = addSetters;
        AddBackingFields = addBackingFields;
        CreateRecord = createRecord;
        CreateAsObservable = createAsObservable;
        SetterVisibility = setterVisibility;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
    }
}
