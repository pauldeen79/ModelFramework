namespace ClassFramework.Pipelines.Builder;

public sealed class PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public PipelineConstructorSettings ConstructorSettings { get; }
    public IPipelineBuilderTypeSettings TypeSettings { get; }
    public Entity.PipelineSettings EntitySettings { get; }

    public bool IsForAbstractBuilder { get; }

    public bool EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    public bool AddNullChecks => EntitySettings.NullCheckSettings.AddNullChecks;
    public bool UseExceptionThrowIfNull => EntitySettings.NullCheckSettings.UseExceptionThrowIfNull;
    public bool EnableInheritance => EntitySettings.InheritanceSettings.EnableInheritance;
    public bool AddBackingFields => EntitySettings.GenerationSettings.AddBackingFields;
    public string CollectionTypeName => TypeSettings.NewCollectionTypeName;
    public string NamespaceFormatString => NameSettings.BuilderNamespaceFormatString;
    public string NameFormatString => NameSettings.BuilderNameFormatString;
    public ArgumentValidationType ValidateArguments => EntitySettings.ConstructorSettings.OriginalValidateArguments;
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

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
        => new(NameSettings, InheritanceSettings, ConstructorSettings, (PipelineTypeSettings)TypeSettings, EntitySettings, true);
}
