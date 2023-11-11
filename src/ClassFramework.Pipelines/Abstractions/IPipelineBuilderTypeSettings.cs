namespace ClassFramework.Pipelines.Abstractions;

public interface IPipelineBuilderTypeSettings
{
    string NewCollectionTypeName { get; }
    IReadOnlyCollection<NamespaceMapping> NamespaceMappings { get; }
    IReadOnlyCollection<TypenameMapping> TypenameMappings { get; }
}
