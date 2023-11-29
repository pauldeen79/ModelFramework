namespace ClassFramework.Domain;

public partial record TypeBase
{
    public string GetFullName() => $"{Namespace.GetNamespacePrefix()}{Name}";

    public string GetGenericTypeArgumentsString()
        => GenericTypeArguments.Count > 0
            ? $"<{string.Join(", ", GenericTypeArguments)}>"
            : string.Empty;
}
