namespace ClassFramework.Pipelines.Builder;

public record BuilderPipelineBuilderConstructorSettings
{
    public bool AddCopyConstructor { get; }
    public bool AddConstructorWithAllProperties { get; }
    public bool AddNullChecks { get; }
    public bool SetDefaultValues { get; }

    public BuilderPipelineBuilderConstructorSettings(
        bool addCopyConstructor = false,
        bool addConstructorWithAllProperties = false,
        bool addNullChecks = false,
        bool setDefaultValues = true)
    {
        AddCopyConstructor = addCopyConstructor;
        AddConstructorWithAllProperties = addConstructorWithAllProperties;
        AddNullChecks = addNullChecks;
        SetDefaultValues = setDefaultValues;
    }
}
