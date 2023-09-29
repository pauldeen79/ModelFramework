namespace ClassFramework.Domain;

public partial record Type
{
    public string GetFullName() => Namespace.GetNamespacePrefix() + Name;
}
