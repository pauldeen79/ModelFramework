namespace ClassFramework.Domain;

public partial record ClassMethod
{
    public bool IsInterfaceMethod()
        => Name.StartsWith("I", StringComparison.Ordinal)
        && Name.Length > 1
        && Name.Equals(Name.Substring(1, 1), StringComparison.OrdinalIgnoreCase)
        && Name.Contains('.');
}
