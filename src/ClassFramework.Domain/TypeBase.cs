namespace ClassFramework.Domain;

public partial record TypeBase
{
    public string GetFullName() => Namespace.GetNamespacePrefix() + Name;
}
