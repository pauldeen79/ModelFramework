namespace ClassFramework.Pipelines.Builder;

public record BuilderPipelineBuilderConstructorSettings
{
    public bool AddCopyConstructor { get; }
    public bool SetDefaultValues { get; }

    public BuilderPipelineBuilderConstructorSettings(
        bool addCopyConstructor = false,
        bool setDefaultValues = true)
    {
        AddCopyConstructor = addCopyConstructor;
        SetDefaultValues = setDefaultValues;
    }
}
