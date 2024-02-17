namespace ClassFramework.Pipelines.Reflection;

public sealed class PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public IPipelineBuilderTypeSettings TypeSettings { get; }
    public PipelineBuilderCopySettings CopySettings { get; }
    public PipelineGenerationSettings GenerationSettings { get; }

    public bool EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    public bool AddNullChecks => default;
    public bool UseExceptionThrowIfNull => default;
    public bool EnableInheritance => InheritanceSettings.EnableInheritance;
    public bool AddBackingFields => default;
    public string CollectionTypeName => string.Empty;
    public ArgumentValidationType ValidateArguments => default;
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate => null;

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
