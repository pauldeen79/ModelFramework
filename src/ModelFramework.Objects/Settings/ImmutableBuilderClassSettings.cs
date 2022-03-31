namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassSettings
{
    public ImmutableBuilderClassConstructorSettings ConstructorSettings { get; }
    public ImmutableBuilderClassTypeSettings TypeSettings { get; }
    public bool Poco { get; }
    public bool UseLazyInitialization { get; }
    public bool UseTargetTypeNewExpressions { get; }
    public bool EnableNullableReferenceTypes { get; }
    public string SetMethodNameFormatString { get; }

    public ImmutableBuilderClassSettings(ImmutableBuilderClassTypeSettings? typeSettings = null,
                                         ImmutableBuilderClassConstructorSettings? constructorSettings = null,
                                         bool poco = false,
                                         bool useLazyInitialization = false,
                                         bool useTargetTypeNewExpressions = true,
                                         bool enableNullableReferenceTypes = false,
                                         string setMethodNameFormatString = "With{0}")
    {
        TypeSettings = typeSettings ?? new ImmutableBuilderClassTypeSettings();
        ConstructorSettings = constructorSettings ?? new ImmutableBuilderClassConstructorSettings();
        Poco = poco;
        UseLazyInitialization = useLazyInitialization;
        UseTargetTypeNewExpressions = useTargetTypeNewExpressions;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        SetMethodNameFormatString = setMethodNameFormatString;
    }

    public ImmutableBuilderClassSettings WithPoco(bool isPoco)
        => new ImmutableBuilderClassSettings(TypeSettings,
                                             ConstructorSettings,
                                             Poco || isPoco,
                                             UseLazyInitialization,
                                             UseTargetTypeNewExpressions,
                                             EnableNullableReferenceTypes,
                                             SetMethodNameFormatString);
}
