namespace ClassFramework.Pipelines.Abstractions;

public interface IPipelineBuilderTypeSettings
{
    string NewCollectionTypeName { get; }
    string CollectionCopyStatementFormatString { get; }
    string CollectionInitializationStatementFormatString { get;}
    string NonCollectionInitializationStatementFormatString { get; }
    bool EnableNullableReferenceTypes { get; }
    IReadOnlyCollection<NamespaceMapping> NamespaceMappings { get; }
    IReadOnlyCollection<TypenameMapping> TypenameMappings { get; }
}
