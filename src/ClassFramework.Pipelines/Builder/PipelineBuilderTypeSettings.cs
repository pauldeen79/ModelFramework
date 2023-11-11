namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderTypeSettings : IPipelineBuilderTypeSettings
{
    public string NewCollectionTypeName { get; }
    public IReadOnlyCollection<NamespaceMapping> NamespaceMappings { get; }
    public IReadOnlyCollection<TypenameMapping> TypenameMappings { get; }

    public PipelineBuilderTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
    {
        NewCollectionTypeName = newCollectionTypeName;
        NamespaceMappings = new ReadOnlyValueCollection<NamespaceMapping>(namespaceMappings ?? Enumerable.Empty<NamespaceMapping>());
        TypenameMappings = new ReadOnlyValueCollection<TypenameMapping>(typenameMappings ?? Enumerable.Empty<TypenameMapping>());
    }
}
