namespace ClassFramework.Pipelines.Shared;

public abstract record PipelineBuilderTypeSettingsBase : IPipelineBuilderTypeSettings
{
    public string NewCollectionTypeName { get; }
    public IReadOnlyCollection<NamespaceMapping> NamespaceMappings { get; }
    public IReadOnlyCollection<TypenameMapping> TypenameMappings { get; }

    protected PipelineBuilderTypeSettingsBase(
        string newCollectionTypeName,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
    {
        NewCollectionTypeName = newCollectionTypeName.IsNotNull(nameof(newCollectionTypeName));
        NamespaceMappings = new ReadOnlyValueCollection<NamespaceMapping>(namespaceMappings ?? Enumerable.Empty<NamespaceMapping>());
        TypenameMappings = new ReadOnlyValueCollection<TypenameMapping>(typenameMappings ?? Enumerable.Empty<TypenameMapping>());
    }
}
