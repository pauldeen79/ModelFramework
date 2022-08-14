namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassSettings
{
    public ImmutableBuilderClassConstructorSettings ConstructorSettings { get; }
    public ImmutableBuilderClassTypeSettings TypeSettings { get; }
    public ImmutableBuilderClassNameSettings NameSettings { get; }
    public ImmutableBuilderClassInheritanceSettings InheritanceSettings { get; }
    public bool UseLazyInitialization { get; }
    public bool CopyPropertyCode { get; }

    public bool IsBuilderForAbstractEntity => InheritanceSettings.EnableEntityInheritance && (InheritanceSettings.BaseClass == null || InheritanceSettings.IsAbstract);
    public bool IsBuilderForOverrideEntity => InheritanceSettings.EnableEntityInheritance && InheritanceSettings.BaseClass != null;
    public bool IsAbstractBuilder => InheritanceSettings.EnableBuilderInheritance && (InheritanceSettings.BaseClass == null || InheritanceSettings.IsAbstract) && !IsForAbstractBuilder;

    public bool IsForAbstractBuilder { get; }

    private ImmutableBuilderClassSettings(ImmutableBuilderClassTypeSettings? typeSettings,
                                          ImmutableBuilderClassConstructorSettings? constructorSettings,
                                          ImmutableBuilderClassNameSettings? nameSettings,
                                          ImmutableBuilderClassInheritanceSettings? inheritanceSettings,
                                          bool useLazyInitialization,
                                          bool copyPropertyCode,
                                          bool isForAbstractBuilder)
    {
        TypeSettings = typeSettings ?? new();
        ConstructorSettings = constructorSettings ?? new();
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        UseLazyInitialization = useLazyInitialization;
        CopyPropertyCode = copyPropertyCode;
        IsForAbstractBuilder = isForAbstractBuilder;
    }

    public ImmutableBuilderClassSettings(ImmutableBuilderClassTypeSettings? typeSettings = null,
                                         ImmutableBuilderClassConstructorSettings? constructorSettings = null,
                                         ImmutableBuilderClassNameSettings? nameSettings = null,
                                         ImmutableBuilderClassInheritanceSettings? inheritanceSettings = null,
                                         bool useLazyInitialization = false,
                                         bool copyPropertyCode = true)
        : this(typeSettings, constructorSettings, nameSettings, inheritanceSettings, useLazyInitialization, copyPropertyCode, false)
    {
    }

    public ImmutableBuilderClassSettings ForAbstractBuilder()
        => new(TypeSettings, ConstructorSettings, NameSettings, InheritanceSettings, UseLazyInitialization, CopyPropertyCode, true);
}
