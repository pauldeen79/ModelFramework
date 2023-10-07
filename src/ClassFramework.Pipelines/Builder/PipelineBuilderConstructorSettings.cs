namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderConstructorSettings
{
    public bool AddCopyConstructor { get; }
    public bool SetDefaultValues { get; }

    public PipelineBuilderConstructorSettings(
        bool addCopyConstructor = false,
        bool setDefaultValues = true)
    {
        AddCopyConstructor = addCopyConstructor;
        SetDefaultValues = setDefaultValues;
    }
}
