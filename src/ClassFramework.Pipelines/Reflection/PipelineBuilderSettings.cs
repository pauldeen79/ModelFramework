namespace ClassFramework.Pipelines.Reflection;

public sealed record PipelineBuilderSettings : IPipelineGenerationSettings
{
    public PipelineBuilderNameSettings NameSettings { get; }
    public PipelineBuilderInheritanceSettings InheritanceSettings { get; }
    public PipelineBuilderTypeSettings TypeSettings { get; }
    public PipelineBuilderCopySettings CopySettings { get; }
    public PipelineBuilderGenerationSettings GenerationSettings { get; }

    bool IPipelineGenerationSettings.EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    bool IPipelineGenerationSettings.AddNullChecks => default;
    bool IPipelineGenerationSettings.EnableInheritance => InheritanceSettings.EnableInheritance;
    bool IPipelineGenerationSettings.AddBackingFields => default;
    string IPipelineGenerationSettings.CollectionTypeName => string.Empty;
    ArgumentValidationType IPipelineGenerationSettings.ValidateArguments => default;
    Func<IParentTypeContainer, TypeBase, bool>? IPipelineGenerationSettings.InheritanceComparisonDelegate => null;

    public PipelineBuilderSettings(
    PipelineBuilderNameSettings? nameSettings = null,
    PipelineBuilderInheritanceSettings? inheritanceSettings = null,
    PipelineBuilderTypeSettings? typeSettings = null,
    PipelineBuilderCopySettings? copySettings = null,
    PipelineBuilderGenerationSettings? generationSettings = null)
    {
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        TypeSettings = typeSettings ?? new();
        CopySettings = copySettings ?? new();
        GenerationSettings = generationSettings ?? new();
    }
}
