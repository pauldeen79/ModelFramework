namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineBuilderTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(newCollectionTypeName, namespaceMappings, typenameMappings)
    {
    }
}
