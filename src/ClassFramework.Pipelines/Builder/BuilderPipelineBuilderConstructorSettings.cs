namespace ClassFramework.Pipelines.Builder;

public record BuilderPipelineBuilderConstructorSettings
{
    public bool AddCopyConstructor { get; }
    public bool AddNullChecks { get; }
    public bool SetDefaultValues { get; }

    public BuilderPipelineBuilderConstructorSettings(
        bool addCopyConstructor = false,
        bool addNullChecks = false,
        bool setDefaultValues = true)
    {
        AddCopyConstructor = addCopyConstructor;
        AddNullChecks = addNullChecks;
        SetDefaultValues = setDefaultValues;
    }
}
