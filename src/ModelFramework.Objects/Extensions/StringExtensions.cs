﻿namespace ModelFramework.Objects.Extensions;

public static class StringExtensions
{
    public static bool ContainsAny(this string instance, params string[] verbs)
        => verbs.Any(instance.Contains);

    public static string ConvertTypeNameToArray(this string typeName)
        => $"{typeName.GetGenericArguments()}[]";

    public static string FixCollectionTypeName(this string typeName, string newCollectionTypeName)
    {
        var fixedTypeName = typeName.FixTypeName();
        return fixedTypeName.IsCollectionTypeName()
                && !string.IsNullOrEmpty(newCollectionTypeName)
                && !string.IsNullOrEmpty(fixedTypeName.GetGenericArguments())
                    ? $"{newCollectionTypeName}<{fixedTypeName.GetGenericArguments()}>"
                    : fixedTypeName;
    }

    public static bool IsCollectionTypeName(this string typeName)
        => typeName.ContainsAny
        (
            "Enumerable<",
            "List<",
            "Collection<",
            "Array<"
        ) || typeName.EndsWith("[]");

    public static string RemoveInterfacePrefix(this string name)
    {
        var index = name.IndexOf(".");
        if (index == -1)
        {
            return name;
        }
        return name.Substring(index + 1);
    }

    public static string AppendNullableAnnotation(this string instance,
                                                  ITypeContainer typeContainer,
                                                  bool enableNullableReferenceTypes)
        => !string.IsNullOrEmpty(instance)
            && !instance.StartsWith("System.Nullable")
            && !instance.EndsWith("?")
            && typeContainer.IsNullable
            && enableNullableReferenceTypes
                ? $"{instance}?"
                : instance;

    public static string AbbreviateNamespaces(this string instance,
                                              IEnumerable<string> namespacesToAbbreviate)
    {
        foreach (var ns in namespacesToAbbreviate)
        {
            if (instance.GetNamespaceWithDefault() == ns)
            {
                return instance.GetClassName();
            }
        }

        return instance;
    }

    public static string GetNamespacePrefix(this string instance)
        => string.IsNullOrEmpty(instance)
            ? string.Empty
            : $"{instance}.";
}
