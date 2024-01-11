namespace ClassFramework.Pipelines.Interface;

public record PipelineTypeSettings : IPipelineBuilderTypeSettings
{
    public IReadOnlyCollection<NamespaceMapping> NamespaceMappings { get; }
    public IReadOnlyCollection<TypenameMapping> TypenameMappings { get; }

    string IPipelineBuilderTypeSettings.NewCollectionTypeName => throw new NotImplementedException();

    bool IPipelineBuilderTypeSettings.EnableNullableReferenceTypes => throw new NotImplementedException();

    IReadOnlyCollection<NamespaceMapping> IPipelineBuilderTypeSettings.NamespaceMappings => NamespaceMappings;
    IReadOnlyCollection<TypenameMapping> IPipelineBuilderTypeSettings.TypenameMappings => TypenameMappings;

    public PipelineTypeSettings(
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
    {
        NamespaceMappings = new ReadOnlyValueCollection<NamespaceMapping>(namespaceMappings ?? Enumerable.Empty<NamespaceMapping>());
        TypenameMappings = new ReadOnlyValueCollection<TypenameMapping>(typenameMappings ?? Enumerable.Empty<TypenameMapping>());
    }
}
