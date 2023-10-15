namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderTypeSettings
{
    public string NewCollectionTypeName { get; }

    public PipelineBuilderTypeSettings(string newCollectionTypeName = "System.Collections.Generic.List")
    {
        NewCollectionTypeName = newCollectionTypeName;
    }
}
