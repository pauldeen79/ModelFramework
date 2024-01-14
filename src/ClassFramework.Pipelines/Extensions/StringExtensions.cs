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

        var typeNameMapping = pipelineBuilderTypeSettings.TypenameMappings.FirstOrDefault(x => x.SourceTypeName == typeName);
        if (typeNameMapping is not null)
        {
            // i.e. TSource => TTarget
            return typeNameMapping.TargetTypeName;
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

        return typeName;
    }

    public static string MapNamespace(this string? ns, IPipelineBuilderTypeSettings pipelineBuilderTypeSettings)
    {
        pipelineBuilderTypeSettings = pipelineBuilderTypeSettings.IsNotNull(nameof(pipelineBuilderTypeSettings));

        if (!string.IsNullOrEmpty(ns))
        {
            // i.e. SourceNamespace.T => TargetNamespace.T
            var namespaceMapping = pipelineBuilderTypeSettings.NamespaceMappings.FirstOrDefault(x => x.SourceNamespace == ns);
            if (namespaceMapping is not null)
            {
                return namespaceMapping.TargetNamespace;
            }
        }

        return ns ?? string.Empty;
    }

    public static string FixNullableTypeName(this string typeName, ITypeContainer typeContainer)
    {
        typeContainer = typeContainer.IsNotNull(nameof(typeContainer));

        if (!typeName.StartsWith("System.Nullable", StringComparison.Ordinal) && typeContainer.IsNullable && typeContainer.IsValueType)
        {
            return typeof(Nullable<>).ReplaceGenericTypeName(typeName);
        }

        return typeName;
    }

    public static string RemoveInterfacePrefixI(this string typeName)
        => typeName.StartsWith("I")
        && typeName.Length > 2
        && typeName.Substring(1, 1).Equals(typeName.Substring(1, 1), StringComparison.OrdinalIgnoreCase)
            ? typeName.Substring(1)
            : typeName;
}
