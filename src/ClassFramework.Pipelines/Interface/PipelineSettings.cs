namespace ClassFramework.Pipelines.Interface;

public sealed record PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineTypeSettings TypeSettings { get; }
    public PipelineBuilderCopySettings CopySettings { get; }
    public PipelineGenerationSettings GenerationSettings { get; }
    public PipelineBuilderNullCheckSettings NullCheckSettings { get; }

    bool IPipelineGenerationSettings.EnableNullableReferenceTypes => false;
    bool IPipelineGenerationSettings.AddNullChecks => false;
    bool IPipelineGenerationSettings.EnableInheritance => false;
    bool IPipelineGenerationSettings.AddBackingFields => false;
    string IPipelineGenerationSettings.CollectionTypeName => string.Empty;
    ArgumentValidationType IPipelineGenerationSettings.ValidateArguments => default;
    Func<IParentTypeContainer, IType, bool>? IPipelineGenerationSettings.InheritanceComparisonDelegate => default;

    public PipelineSettings(
        PipelineNameSettings? nameSettings = null,
        PipelineTypeSettings? typeSettings = null,
        PipelineBuilderCopySettings? copySettings = null,
        PipelineGenerationSettings? generationSettings = null,
        PipelineBuilderNullCheckSettings? nullCheckSettings = null)
    {
        NameSettings = nameSettings ?? new();
        TypeSettings = typeSettings ?? new();
        CopySettings = copySettings ?? new();
        GenerationSettings = generationSettings ?? new();
        NullCheckSettings = nullCheckSettings ?? new();
    }
}
