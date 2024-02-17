namespace ClassFramework.Pipelines.Interface;

public sealed class PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public PipelineTypeSettings TypeSettings { get; }
    public PipelineBuilderCopySettings CopySettings { get; }
    public PipelineGenerationSettings GenerationSettings { get; }

    public bool EnableNullableReferenceTypes => false;
    public bool AddNullChecks => false;
    public bool UseExceptionThrowIfNull => false;
    public bool EnableInheritance => InheritanceSettings.EnableInheritance;
    public bool AddBackingFields => false;
    public string CollectionTypeName => string.Empty;
    public ArgumentValidationType ValidateArguments => default;
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

    public PipelineSettings(
        PipelineNameSettings? nameSettings = null,
        PipelineInheritanceSettings? inheritanceSettings = null,
        PipelineTypeSettings? typeSettings = null,
        PipelineBuilderCopySettings? copySettings = null,
        PipelineGenerationSettings? generationSettings = null)
    {
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        TypeSettings = typeSettings ?? new();
        CopySettings = copySettings ?? new();
        GenerationSettings = generationSettings ?? new();
    }
}
