namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassTypeSettings
{
    public string NewCollectionTypeName { get; }
    public Func<ITypeBase, bool, string>? FormatInstanceTypeNameDelegate { get; }

    public ImmutableBuilderClassTypeSettings(string newCollectionTypeName = "System.Collections.Generic.List",
                                             Func<ITypeBase, bool, string>? formatInstanceTypeNameDelegate = null)
    {
        NewCollectionTypeName = newCollectionTypeName;
        FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
    }
}
