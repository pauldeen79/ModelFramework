namespace ClassFramework.Pipelines.Extensions;

public static class StringExtensions
{
    public static string MapTypeName(this string typeName, IPipelineBuilderTypeSettings pipelineBuilderTypeSettings)
    {
        pipelineBuilderTypeSettings = pipelineBuilderTypeSettings.IsNotNull(nameof(pipelineBuilderTypeSettings));

        if (typeName.IsCollectionTypeName())
        {
            // i.e. IEnumerable<TSource> => IEnumerable<TTarget> (including collection typename mapping, when available)
            return typeName
                .FixCollectionTypeName(pipelineBuilderTypeSettings.NewCollectionTypeName) // note that this always converts to a generic type :)
                .ReplaceGenericTypeName(MapTypeName(typeName.GetCollectionItemType(), pipelineBuilderTypeSettings)); // so we can safely use ReplaceGenericTypeName here
        }

        var ns = typeName.GetNamespaceWithDefault();
        if (!string.IsNullOrEmpty(ns))
        {
            // i.e. SourceNamespace.T => TargetNamespace.T
            var namespaceMapping = pipelineBuilderTypeSettings.NamespaceMappings.FirstOrDefault(x => x.SourceNamespace == ns);
            if (namespaceMapping is not null)
            {
                return $"{namespaceMapping.TargetNamespace}.{typeName.GetClassName()}";
            }
        }

        var typeNameMapping = pipelineBuilderTypeSettings.TypenameMappings.FirstOrDefault(x => x.SourceTypeName == typeName);
        if (typeNameMapping is not null)
        {
            // i.e. TSource => TTarget
            return typeNameMapping.TargetTypeName;
        }

        return typeName;
    }
}
