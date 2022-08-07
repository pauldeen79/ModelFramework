namespace ModelFramework.Objects.Extensions;

public static class TypeBaseBuilderExtensions
{
    public static string GetFullName(this TypeBaseBuilder instance)
        => instance.Namespace.GetNamespacePrefix() + instance.Name;
}
