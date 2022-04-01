namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassSettings
{
    public ImmutableBuilderClassConstructorSettings ConstructorSettings { get; }
    public ImmutableBuilderClassTypeSettings TypeSettings { get; }
    public ImmutableBuilderClassNameSettings NameSettings { get; }
    public bool UseLazyInitialization { get; }
    public bool EnableNullableReferenceTypes { get; }

    public ImmutableBuilderClassSettings(ImmutableBuilderClassTypeSettings? typeSettings = null,
                                         ImmutableBuilderClassConstructorSettings? constructorSettings = null,
                                         ImmutableBuilderClassNameSettings? nameSettings = null,
                                         bool useLazyInitialization = false,
                                         bool enableNullableReferenceTypes = false)
    {
        TypeSettings = typeSettings ?? new ImmutableBuilderClassTypeSettings();
        ConstructorSettings = constructorSettings ?? new ImmutableBuilderClassConstructorSettings();
        NameSettings = nameSettings ?? new ImmutableBuilderClassNameSettings();
        UseLazyInitialization = useLazyInitialization;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
    }
}
