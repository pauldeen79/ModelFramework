namespace ClassFramework.Pipelines.OverrideEntity;

public class PipelineTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.List",
        bool enableNullableReferenceTypes = false,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(newCollectionTypeName, string.Empty, string.Empty, string.Empty, enableNullableReferenceTypes, namespaceMappings, typenameMappings)
    {
    }
}
