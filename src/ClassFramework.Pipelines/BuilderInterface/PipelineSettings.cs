namespace ClassFramework.Pipelines.BuilderInterface;

public sealed class PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public PipelineTypeSettings TypeSettings { get; }
    public Entity.PipelineSettings EntitySettings { get; }

    bool IPipelineGenerationSettings.EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    bool IPipelineGenerationSettings.AddNullChecks => EntitySettings.NullCheckSettings.AddNullChecks;
    bool IPipelineGenerationSettings.EnableInheritance => EntitySettings.InheritanceSettings.EnableInheritance;
    bool IPipelineGenerationSettings.AddBackingFields => EntitySettings.GenerationSettings.AddBackingFields;
    string IPipelineGenerationSettings.CollectionTypeName => TypeSettings.NewCollectionTypeName;
    ArgumentValidationType IPipelineGenerationSettings.ValidateArguments => EntitySettings.ConstructorSettings.OriginalValidateArguments;
    Func<IParentTypeContainer, IType, bool>? IPipelineGenerationSettings.InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

    public PipelineSettings(
        PipelineNameSettings? nameSettings,
        PipelineInheritanceSettings? inheritanceSettings,
        PipelineTypeSettings? typeSettings,
        Entity.PipelineSettings? entitySettings)
    {
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        TypeSettings = typeSettings ?? new();
        EntitySettings = entitySettings ?? new();
    }
}
