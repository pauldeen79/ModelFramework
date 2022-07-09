namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassSettings
{
    public ImmutableBuilderClassConstructorSettings ConstructorSettings { get; }
    public ImmutableBuilderClassTypeSettings TypeSettings { get; }
    public ImmutableBuilderClassNameSettings NameSettings { get; }
    public ImmutableBuilderClassInheritanceSettings InheritanceSettings { get; }
    public bool UseLazyInitialization { get; }
    public bool EnableNullableReferenceTypes { get; }
    public bool CopyPropertyCode { get; }

    public ImmutableBuilderClassSettings(ImmutableBuilderClassTypeSettings? typeSettings = null,
                                         ImmutableBuilderClassConstructorSettings? constructorSettings = null,
                                         ImmutableBuilderClassNameSettings? nameSettings = null,
                                         ImmutableBuilderClassInheritanceSettings? inheritanceSettings = null,
                                         bool useLazyInitialization = false,
                                         bool enableNullableReferenceTypes = false,
                                         bool copyPropertyCode = true)
    {
        TypeSettings = typeSettings ?? new ImmutableBuilderClassTypeSettings();
        ConstructorSettings = constructorSettings ?? new ImmutableBuilderClassConstructorSettings();
        NameSettings = nameSettings ?? new ImmutableBuilderClassNameSettings();
        InheritanceSettings = inheritanceSettings ?? new ImmutableBuilderClassInheritanceSettings();
        UseLazyInitialization = useLazyInitialization;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        CopyPropertyCode = copyPropertyCode;
    }
}
