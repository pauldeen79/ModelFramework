namespace ClassFramework.Pipelines.Entity;

public record PipelineGenerationSettings
{
    public bool AddSetters { get; }
    public bool AddBackingFields { get; }
    public bool CreateRecord { get; }
    public bool CreateAsObservable { get; }
    public SubVisibility SetterVisibility { get; }
    public bool AllowGenerationWithoutProperties { get; }
    public bool BaseClass { get; }

    public PipelineGenerationSettings(
        bool addSetters = false,
        bool addBackingFields = false,
        bool createRecord = false,
        bool createAsObservable = false,
        SubVisibility setterVisibility = SubVisibility.InheritFromParent,
        bool allowGenerationWithoutProperties = false,
        bool baseClass = false)
    {
        AddSetters = addSetters;
        AddBackingFields = addBackingFields;
        CreateRecord = createRecord;
        CreateAsObservable = createAsObservable;
        SetterVisibility = setterVisibility;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        BaseClass = baseClass;
    }
}
