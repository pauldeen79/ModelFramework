namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderTypeSettings
{
    public string NewCollectionTypeName { get; }
    public Func<TypeBase, bool, string>? FormatInstanceTypeNameDelegate { get; }

    public PipelineBuilderTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.List",
        Func<TypeBase, bool, string>? formatInstanceTypeNameDelegate = null)
    {
        NewCollectionTypeName = newCollectionTypeName;
        FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
    }
}
