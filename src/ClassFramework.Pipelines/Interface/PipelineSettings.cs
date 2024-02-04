namespace ClassFramework.Pipelines.Interface;

public sealed class PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public PipelineTypeSettings TypeSettings { get; }
    public PipelineBuilderCopySettings CopySettings { get; }
    public PipelineGenerationSettings GenerationSettings { get; }

    bool IPipelineGenerationSettings.EnableNullableReferenceTypes => false;
    bool IPipelineGenerationSettings.AddNullChecks => false;
    bool IPipelineGenerationSettings.EnableInheritance => InheritanceSettings.EnableInheritance;
    bool IPipelineGenerationSettings.AddBackingFields => false;
    string IPipelineGenerationSettings.CollectionTypeName => string.Empty;
    ArgumentValidationType IPipelineGenerationSettings.ValidateArguments => default;
    Func<IParentTypeContainer, IType, bool>? IPipelineGenerationSettings.InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

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
