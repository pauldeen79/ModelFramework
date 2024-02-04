namespace ClassFramework.Pipelines.Interface;

public class PipelineTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.List",
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(newCollectionTypeName, default, namespaceMappings, typenameMappings)
    {
    }
}
