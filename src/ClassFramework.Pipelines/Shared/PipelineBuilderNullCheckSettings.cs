namespace ClassFramework.Pipelines.Shared;

public record PipelineBuilderNullCheckSettings
{
    public PipelineBuilderNullCheckSettings(bool addNullChecks = false, bool useExceptionThrowIfNull = false)
    {
        AddNullChecks = addNullChecks;
        UseExceptionThrowIfNull = useExceptionThrowIfNull;
    }

    public bool AddNullChecks { get; }
    public bool UseExceptionThrowIfNull { get; }
}
