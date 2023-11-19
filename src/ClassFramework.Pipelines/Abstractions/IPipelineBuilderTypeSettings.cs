namespace ClassFramework.Pipelines.Abstractions;

public interface IPipelineBuilderTypeSettings
{
    string NewCollectionTypeName { get; }
    bool EnableNullableReferenceTypes { get; }
    IReadOnlyCollection<NamespaceMapping> NamespaceMappings { get; }
    IReadOnlyCollection<TypenameMapping> TypenameMappings { get; }
}
