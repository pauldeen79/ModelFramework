namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassTypeSettings
{
    public string NewCollectionTypeName { get; }
    public bool UseTargetTypeNewExpressions { get; }
    public bool EnableNullableReferenceTypes { get; }
    public Func<ITypeBase, bool, string>? FormatInstanceTypeNameDelegate { get; }

    public ImmutableBuilderClassTypeSettings(string newCollectionTypeName = "System.Collections.Generic.List",
                                             bool useTargetTypeNewExpressions = true,
                                             bool enableNullableReferenceTypes = false,
                                             Func<ITypeBase, bool, string>? formatInstanceTypeNameDelegate = null)
    {
        NewCollectionTypeName = newCollectionTypeName;
        UseTargetTypeNewExpressions = useTargetTypeNewExpressions;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
    }
}
