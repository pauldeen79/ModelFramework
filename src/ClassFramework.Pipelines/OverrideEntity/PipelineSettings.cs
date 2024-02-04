namespace ClassFramework.Pipelines.OverrideEntity;

public sealed class PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public PipelineTypeSettings TypeSettings { get; }
    public PipelineGenerationSettings GenerationSettings { get; }
    public PipelineBuilderNullCheckSettings NullCheckSettings { get; }

    bool IPipelineGenerationSettings.EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    bool IPipelineGenerationSettings.AddNullChecks => NullCheckSettings.AddNullChecks;
    bool IPipelineGenerationSettings.EnableInheritance => InheritanceSettings.EnableInheritance;
    bool IPipelineGenerationSettings.AddBackingFields => default;
    string IPipelineGenerationSettings.CollectionTypeName => string.Empty;
    ArgumentValidationType IPipelineGenerationSettings.ValidateArguments => default;
    Func<IParentTypeContainer, IType, bool>? IPipelineGenerationSettings.InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

    public PipelineSettings(
        PipelineNameSettings? nameSettings = null, 
        PipelineInheritanceSettings? inheritanceSettings = null,
        PipelineTypeSettings? typeSettings = null,
        PipelineGenerationSettings? generationSettings = null,
        PipelineBuilderNullCheckSettings? nullCheckSettings = null)
    {
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        TypeSettings = typeSettings ?? new();
        GenerationSettings = generationSettings ?? new();
        NullCheckSettings = nullCheckSettings ?? new();
    }
}
