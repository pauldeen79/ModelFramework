namespace ClassFramework.Pipelines.OverrideEntity;

public sealed class PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public PipelineTypeSettings TypeSettings { get; }
    public PipelineGenerationSettings GenerationSettings { get; }
    public PipelineBuilderNullCheckSettings NullCheckSettings { get; }

    public bool EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    public bool AddNullChecks => NullCheckSettings.AddNullChecks;
    public bool UseExceptionThrowIfNull => NullCheckSettings.UseExceptionThrowIfNull;
    public bool EnableInheritance => InheritanceSettings.EnableInheritance;
    public bool AddBackingFields => default;
    public string CollectionTypeName => string.Empty;
    public ArgumentValidationType ValidateArguments => default;
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

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
