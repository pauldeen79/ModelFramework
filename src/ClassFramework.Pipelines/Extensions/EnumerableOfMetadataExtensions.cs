namespace ClassFramework.Pipelines.Extensions;

public static class EnumerableOfMetadataExtensions
{
    public static IEnumerable<Metadata> WithMappingMetadata(
        this IEnumerable<Metadata> metadata,
        string typeName,
        IPipelineBuilderTypeSettings pipelineBuilderTypeSettings)
    {
        typeName = typeName.IsNotNull(nameof(typeName));
        pipelineBuilderTypeSettings = pipelineBuilderTypeSettings.IsNotNull(nameof(pipelineBuilderTypeSettings));

        var typeNameMapping = pipelineBuilderTypeSettings.TypenameMappings.FirstOrDefault(x => x.SourceTypeName == typeName);
        if (typeNameMapping is not null)
        {
            return metadata.Concat(typeNameMapping.Metadata);
        }

        var ns = typeName.GetNamespaceWithDefault();
        if (!string.IsNullOrEmpty(ns))
        {
            var namespaceMapping = pipelineBuilderTypeSettings.NamespaceMappings.FirstOrDefault(x => x.SourceNamespace == ns);
            if (namespaceMapping is not null)
            {
                return metadata.Concat(namespaceMapping.Metadata);
            }
        }

        return metadata;
    }
}
