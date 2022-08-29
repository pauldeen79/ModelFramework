namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassSettings
{
    public ImmutableBuilderClassConstructorSettings ConstructorSettings { get; }
    public ImmutableBuilderClassTypeSettings TypeSettings { get; }
    public ImmutableBuilderClassNameSettings NameSettings { get; }
    public ImmutableBuilderClassInheritanceSettings InheritanceSettings { get; }
    public ImmutableBuilderClassGenerationSettings GenerationSettings { get; }

    public bool IsBuilderForAbstractEntity => InheritanceSettings.EnableEntityInheritance && (InheritanceSettings.BaseClass == null || InheritanceSettings.IsAbstract);
    public bool IsBuilderForOverrideEntity => InheritanceSettings.EnableEntityInheritance && InheritanceSettings.BaseClass != null;
    public bool IsAbstractBuilder => InheritanceSettings.EnableBuilderInheritance && (InheritanceSettings.BaseClass == null || InheritanceSettings.IsAbstract) && !IsForAbstractBuilder;

    public bool IsForAbstractBuilder { get; }

    private ImmutableBuilderClassSettings(ImmutableBuilderClassTypeSettings? typeSettings,
                                          ImmutableBuilderClassConstructorSettings? constructorSettings,
                                          ImmutableBuilderClassNameSettings? nameSettings,
                                          ImmutableBuilderClassInheritanceSettings? inheritanceSettings,
                                          ImmutableBuilderClassGenerationSettings? generationSettings,
                                          bool isForAbstractBuilder)
    {
        TypeSettings = typeSettings ?? new();
        ConstructorSettings = constructorSettings ?? new();
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        GenerationSettings = generationSettings ?? new();

        IsForAbstractBuilder = isForAbstractBuilder;
    }

    public ImmutableBuilderClassSettings(ImmutableBuilderClassTypeSettings? typeSettings = null,
                                         ImmutableBuilderClassConstructorSettings? constructorSettings = null,
                                         ImmutableBuilderClassNameSettings? nameSettings = null,
                                         ImmutableBuilderClassInheritanceSettings? inheritanceSettings = null,
                                         ImmutableBuilderClassGenerationSettings? generationSettings = null)
        : this(typeSettings, constructorSettings, nameSettings, inheritanceSettings, generationSettings, false)
    {
    }

    public ImmutableBuilderClassSettings ForAbstractBuilder()
        => new(TypeSettings, ConstructorSettings, NameSettings, InheritanceSettings, GenerationSettings, true);
}
