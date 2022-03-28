namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassSettings
{
    public string NewCollectionTypeName { get; }
    public ImmutableBuilderClassConstructorSettings ConstructorSettings { get; }
    public bool Poco { get; }
    public bool UseLazyInitialization { get; }
    public bool UseTargetTypeNewExpressions { get; }
    public bool EnableNullableReferenceTypes { get; }
    public string SetMethodNameFormatString { get; }
    public Func<ITypeBase, bool, string>? FormatInstanceTypeNameDelegate { get; }

    public ImmutableBuilderClassSettings(string newCollectionTypeName = "System.Collections.Generic.List",
                                         ImmutableBuilderClassConstructorSettings? constructorSettings = null,
                                         bool poco = false,
                                         bool useLazyInitialization = false,
                                         bool useTargetTypeNewExpressions = true,
                                         bool enableNullableReferenceTypes = false,
                                         string setMethodNameFormatString = "With{0}",
                                         Func<ITypeBase, bool, string>? formatInstanceTypeNameDelegate = null)
    {
        NewCollectionTypeName = newCollectionTypeName;
        ConstructorSettings = constructorSettings ?? new ImmutableBuilderClassConstructorSettings();
        Poco = poco;
        UseLazyInitialization = useLazyInitialization;
        UseTargetTypeNewExpressions = useTargetTypeNewExpressions;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        SetMethodNameFormatString = setMethodNameFormatString;
        FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
    }

    public ImmutableBuilderClassSettings WithPoco(bool isPoco)
        => new ImmutableBuilderClassSettings(NewCollectionTypeName,
                                             ConstructorSettings,
                                             Poco || isPoco,
                                             UseLazyInitialization,
                                             UseTargetTypeNewExpressions,
                                             EnableNullableReferenceTypes,
                                             SetMethodNameFormatString,
                                             FormatInstanceTypeNameDelegate);
}
