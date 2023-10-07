namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderSettings
{
    public PipelineBuilderNameSettings NameSettings { get; }
    public PipelineBuilderInheritanceSettings InheritanceSettings { get; }
    public PipelineBuilderConstructorSettings ConstructorSettings { get; }
    public PipelineBuilderTypeSettings TypeSettings { get; }
    public PipelineBuilderGenerationSettings GenerationSettings { get; }
    public ImmutableClassPipelineBuilderSettings ClassSettings { get; }

    public bool IsBuilderForAbstractEntity => InheritanceSettings.EnableEntityInheritance && (InheritanceSettings.BaseClass is null || InheritanceSettings.IsAbstract);
    public bool IsBuilderForOverrideEntity => InheritanceSettings.EnableEntityInheritance && InheritanceSettings.BaseClass is not null;
    public bool IsAbstractBuilder => InheritanceSettings.EnableBuilderInheritance && (InheritanceSettings.BaseClass is null || InheritanceSettings.IsAbstract) && !IsForAbstractBuilder;

    public bool IsForAbstractBuilder { get; }

    private PipelineBuilderSettings(
        PipelineBuilderNameSettings? nameSettings,
        PipelineBuilderInheritanceSettings? inheritanceSettings,
        PipelineBuilderConstructorSettings? constructorSettings,
        PipelineBuilderTypeSettings? typeSettings,
        PipelineBuilderGenerationSettings? generationSettings,
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

    public PipelineBuilderSettings(
        PipelineBuilderNameSettings? nameSettings = null,
        PipelineBuilderInheritanceSettings? inheritanceSettings = null,
        PipelineBuilderConstructorSettings? constructorSettings = null,
        PipelineBuilderTypeSettings? typeSettings = null,
        PipelineBuilderGenerationSettings? generationSettings = null,
        ImmutableClassPipelineBuilderSettings? classSettings = null)
        : this(nameSettings, inheritanceSettings, constructorSettings, typeSettings, generationSettings, classSettings, false)
    {
    }

    public PipelineBuilderSettings ForAbstractBuilder()
        => new(NameSettings, InheritanceSettings, ConstructorSettings, TypeSettings, GenerationSettings, ClassSettings, true);
}
