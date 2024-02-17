namespace ClassFramework.Pipelines.Shared;

public abstract class PipelineBuilderTypeSettingsBase : IPipelineBuilderTypeSettings
{
    public string NewCollectionTypeName { get; }
    public string CollectionCopyStatementFormatString { get; }
    public string CollectionInitializationStatementFormatString { get; }
    public string NonCollectionInitializationStatementFormatString { get; }
    public bool EnableNullableReferenceTypes { get; }
    public IReadOnlyCollection<NamespaceMapping> NamespaceMappings { get; }
    public IReadOnlyCollection<TypenameMapping> TypenameMappings { get; }

    protected PipelineBuilderTypeSettingsBase(
        string newCollectionTypeName,
        string collectionCopyStatementFormatString,
        string collectionInitializationStatementFormatString,
        string nonCollectionInitializationStatementFormatString,
        bool enableNullableReferenceTypes = false,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
    {
        NewCollectionTypeName = newCollectionTypeName.IsNotNull(nameof(newCollectionTypeName));
        CollectionCopyStatementFormatString = collectionCopyStatementFormatString.IsNotNull(nameof(collectionCopyStatementFormatString));
        NonCollectionInitializationStatementFormatString = nonCollectionInitializationStatementFormatString.IsNotNull(nameof(nonCollectionInitializationStatementFormatString));
        CollectionInitializationStatementFormatString = collectionInitializationStatementFormatString.IsNotNull(nameof(collectionInitializationStatementFormatString));
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        NamespaceMappings = new ReadOnlyValueCollection<NamespaceMapping>(namespaceMappings ?? Enumerable.Empty<NamespaceMapping>());
        TypenameMappings = new ReadOnlyValueCollection<TypenameMapping>(typenameMappings ?? Enumerable.Empty<TypenameMapping>());
    }
}
