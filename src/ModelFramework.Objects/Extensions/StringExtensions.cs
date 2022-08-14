namespace ModelFramework.Objects.Extensions;

public static class StringExtensions
{
    public static bool ContainsAny(this string instance, params string[] verbs)
        => verbs.Any(instance.Contains);

    public static string ConvertTypeNameToArray(this string typeName)
        => $"{typeName.FixTypeName().GetGenericArguments()}[]";

    public static string FixCollectionTypeName(this string typeName, string newCollectionTypeName)
        => typeName.IsCollectionTypeName()
            && !string.IsNullOrEmpty(newCollectionTypeName)
            && !string.IsNullOrEmpty(typeName.FixTypeName().GetGenericArguments())
                ? $"{newCollectionTypeName}<{typeName.FixTypeName().GetGenericArguments()}>"
                : typeName.FixTypeName();

    public static bool IsCollectionTypeName(this string typeName)
        => typeName.FixTypeName().ContainsAny
        (
            "Enumerable<",
            "List<",
            "Collection<",
            "Array<"
        );

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
