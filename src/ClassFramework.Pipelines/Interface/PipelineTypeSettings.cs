namespace ClassFramework.Pipelines.Interface;

public class PipelineTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.List",
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(newCollectionTypeName, string.Empty, string.Empty, string.Empty, default, namespaceMappings, typenameMappings)
    {
    }
}
