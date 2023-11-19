namespace ClassFramework.Pipelines.Builder;

public sealed record PipelineBuilderSettings : IPipelineGenerationSettings
{
    public PipelineBuilderNameSettings NameSettings { get; }
    public PipelineBuilderInheritanceSettings InheritanceSettings { get; }
    public PipelineBuilderConstructorSettings ConstructorSettings { get; }
    public PipelineBuilderTypeSettings TypeSettings { get; }
    public Entity.PipelineBuilderSettings EntitySettings { get; }

    public bool IsForAbstractBuilder { get; }

    bool IPipelineGenerationSettings.EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    bool IPipelineGenerationSettings.AddNullChecks => EntitySettings.NullCheckSettings.AddNullChecks;
    bool IPipelineGenerationSettings.EnableInheritance => EntitySettings.InheritanceSettings.EnableInheritance;
    string IPipelineGenerationSettings.CollectionTypeName => TypeSettings.NewCollectionTypeName;
    ArgumentValidationType IPipelineGenerationSettings.ValidateArguments => EntitySettings.ConstructorSettings.OriginalValidateArguments;
    Func<IParentTypeContainer, TypeBase, bool>? IPipelineGenerationSettings.InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

    private PipelineBuilderSettings(
        PipelineBuilderNameSettings? nameSettings,
        PipelineBuilderInheritanceSettings? inheritanceSettings,
        PipelineBuilderConstructorSettings? constructorSettings,
        PipelineBuilderTypeSettings? typeSettings,
        Entity.PipelineBuilderSettings? entitySettings,
        bool isForAbstractBuilder)
    {
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        ConstructorSettings = constructorSettings ?? new();
        TypeSettings = typeSettings ?? new();
        EntitySettings = entitySettings ?? new();
        IsForAbstractBuilder = isForAbstractBuilder;
    }

    public PipelineBuilderSettings(
        PipelineBuilderNameSettings? nameSettings = null,
        PipelineBuilderInheritanceSettings? inheritanceSettings = null,
        PipelineBuilderConstructorSettings? constructorSettings = null,
        PipelineBuilderTypeSettings? typeSettings = null,
        Entity.PipelineBuilderSettings? entitySettings = null)
        : this(nameSettings, inheritanceSettings, constructorSettings, typeSettings, entitySettings, false)
    {
    }

    public PipelineBuilderSettings ForAbstractBuilder()
        => new(NameSettings, InheritanceSettings, ConstructorSettings, TypeSettings, EntitySettings, true);
}
