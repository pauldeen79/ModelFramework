namespace ClassFramework.Pipelines.Builder;

public record BuilderPipelineBuilderSettings
{
    public BuilderPipelineBuilderNameSettings NameSettings { get; }
    public BuilderPipelineBuilderInheritanceSettings InheritanceSettings { get; }
    public BuilderPipelineBuilderConstructorSettings ConstructorSettings { get; }
    public BuilderPipelineBuilderTypeSettings TypeSettings { get; }
    public BuilderPipelineBuilderGenerationSettings GenerationSettings { get; }
    public ImmutableClassPipelineBuilderSettings ClassSettings { get; }

    public bool IsBuilderForAbstractEntity => InheritanceSettings.EnableEntityInheritance && (InheritanceSettings.BaseClass == null || InheritanceSettings.IsAbstract);
    public bool IsBuilderForOverrideEntity => InheritanceSettings.EnableEntityInheritance && InheritanceSettings.BaseClass != null;
    public bool IsAbstractBuilder => InheritanceSettings.EnableBuilderInheritance && (InheritanceSettings.BaseClass == null || InheritanceSettings.IsAbstract) && !IsForAbstractBuilder;

    public bool IsForAbstractBuilder { get; }

    private BuilderPipelineBuilderSettings(
        BuilderPipelineBuilderNameSettings? nameSettings,
        BuilderPipelineBuilderInheritanceSettings? inheritanceSettings,
        BuilderPipelineBuilderConstructorSettings? constructorSettings,
        BuilderPipelineBuilderTypeSettings? typeSettings,
        BuilderPipelineBuilderGenerationSettings? generationSettings,
        ImmutableClassPipelineBuilderSettings? classSettings,
        bool isForAbstractBuilder)
    {
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        ConstructorSettings = constructorSettings ?? new();
        TypeSettings = typeSettings ?? new();
        GenerationSettings = generationSettings ?? new();
        ClassSettings = classSettings ?? new();
        IsForAbstractBuilder = isForAbstractBuilder;
    }

    public BuilderPipelineBuilderSettings(
        BuilderPipelineBuilderNameSettings? nameSettings = null,
        BuilderPipelineBuilderInheritanceSettings? inheritanceSettings = null,
        BuilderPipelineBuilderConstructorSettings? constructorSettings = null,
        BuilderPipelineBuilderTypeSettings? typeSettings = null,
        BuilderPipelineBuilderGenerationSettings? generationSettings = null,
        ImmutableClassPipelineBuilderSettings? classSettings = null)
        : this(nameSettings, inheritanceSettings, constructorSettings, typeSettings, generationSettings, classSettings, false)
    {
    }

    public BuilderPipelineBuilderSettings ForAbstractBuilder()
        => new(NameSettings, InheritanceSettings, ConstructorSettings, TypeSettings, GenerationSettings, ClassSettings, true);
}
