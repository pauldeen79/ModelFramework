namespace ClassFramework.Pipelines.Builder;

public sealed class PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public PipelineConstructorSettings ConstructorSettings { get; }
    public PipelineTypeSettings TypeSettings { get; }
    public Entity.PipelineSettings EntitySettings { get; }

    public bool IsForAbstractBuilder { get; }

    bool IPipelineGenerationSettings.EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    bool IPipelineGenerationSettings.AddNullChecks => EntitySettings.NullCheckSettings.AddNullChecks;
    bool IPipelineGenerationSettings.EnableInheritance => EntitySettings.InheritanceSettings.EnableInheritance;
    bool IPipelineGenerationSettings.AddBackingFields => EntitySettings.GenerationSettings.AddBackingFields;
    string IPipelineGenerationSettings.CollectionTypeName => TypeSettings.NewCollectionTypeName;
    ArgumentValidationType IPipelineGenerationSettings.ValidateArguments => EntitySettings.ConstructorSettings.OriginalValidateArguments;
    Func<IParentTypeContainer, IType, bool>? IPipelineGenerationSettings.InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

    private PipelineSettings(
        PipelineNameSettings? nameSettings,
        PipelineInheritanceSettings? inheritanceSettings,
        PipelineConstructorSettings? constructorSettings,
        PipelineTypeSettings? typeSettings,
        Entity.PipelineSettings? entitySettings,
        bool isForAbstractBuilder)
    {
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        ConstructorSettings = constructorSettings ?? new();
        TypeSettings = typeSettings ?? new();
        EntitySettings = entitySettings ?? new();
        IsForAbstractBuilder = isForAbstractBuilder;
    }

    public PipelineSettings(
        PipelineNameSettings? nameSettings = null,
        PipelineInheritanceSettings? inheritanceSettings = null,
        PipelineConstructorSettings? constructorSettings = null,
        PipelineTypeSettings? typeSettings = null,
        Entity.PipelineSettings? entitySettings = null)
        : this(nameSettings, inheritanceSettings, constructorSettings, typeSettings, entitySettings, false)
    {
    }

    public PipelineSettings ForAbstractBuilder()
        => new(NameSettings, InheritanceSettings, ConstructorSettings, TypeSettings, EntitySettings, true);
}
