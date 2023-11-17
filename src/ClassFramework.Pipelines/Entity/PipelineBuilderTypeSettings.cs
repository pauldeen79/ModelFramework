namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineBuilderTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.List",
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(newCollectionTypeName, namespaceMappings, typenameMappings)
    {
    }
}
