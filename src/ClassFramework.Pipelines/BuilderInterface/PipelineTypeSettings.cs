namespace ClassFramework.Pipelines.BuilderInterface;

public class PipelineTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        string collectionCopyStatementFormatString = "foreach (var item in {NamePascalCsharpFriendlyName}) instance.{Name}.Add(item);",
        bool enableNullableReferenceTypes = false,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(newCollectionTypeName, enableNullableReferenceTypes, namespaceMappings, typenameMappings)
    {
        CollectionCopyStatementFormatString = collectionCopyStatementFormatString;
    }

    public string CollectionCopyStatementFormatString { get; }
}
