namespace ClassFramework.Pipelines.Builder;

public record PipelineConstructorSettings
{
    public bool AddCopyConstructor { get; }
    public bool SetDefaultValues { get; }

    public PipelineConstructorSettings(
        bool addCopyConstructor = true,
        bool setDefaultValues = true)
    {
        AddCopyConstructor = addCopyConstructor;
        SetDefaultValues = setDefaultValues;
    }
}
