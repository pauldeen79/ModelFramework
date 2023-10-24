namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderSettings
{
    public PipelineBuilderNameSettings NameSettings { get; }
    public PipelineBuilderInheritanceSettings InheritanceSettings { get; }
    public PipelineBuilderConstructorSettings ConstructorSettings { get; }
    public PipelineBuilderTypeSettings TypeSettings { get; }
    public PipelineBuilderGenerationSettings GenerationSettings { get; }
    public Entity.PipelineBuilderSettings ClassSettings { get; }

    public bool IsForAbstractBuilder { get; }

    private PipelineBuilderSettings(
        PipelineBuilderNameSettings? nameSettings,
        PipelineBuilderInheritanceSettings? inheritanceSettings,
        PipelineBuilderConstructorSettings? constructorSettings,
        PipelineBuilderTypeSettings? typeSettings,
        PipelineBuilderGenerationSettings? generationSettings,
        Entity.PipelineBuilderSettings? classSettings,
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
        Entity.PipelineBuilderSettings? classSettings = null)
        : this(nameSettings, inheritanceSettings, constructorSettings, typeSettings, generationSettings, classSettings, false)
    {
    }

    public PipelineBuilderSettings ForAbstractBuilder()
        => new(NameSettings, InheritanceSettings, ConstructorSettings, TypeSettings, GenerationSettings, ClassSettings, true);
}
