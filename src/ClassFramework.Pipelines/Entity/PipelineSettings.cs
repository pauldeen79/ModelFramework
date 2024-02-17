namespace ClassFramework.Pipelines.Entity;

public sealed class PipelineSettings : IPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineConstructorSettings ConstructorSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public PipelineTypeSettings TypeSettings { get; }
    public PipelineBuilderCopySettings CopySettings { get; }
    public PipelineGenerationSettings GenerationSettings { get; }
    public PipelineBuilderNullCheckSettings NullCheckSettings { get; }

    public bool EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    public bool AddNullChecks => NullCheckSettings.AddNullChecks;
    public bool UseExceptionThrowIfNull => NullCheckSettings.UseExceptionThrowIfNull;
    public bool EnableInheritance => InheritanceSettings.EnableInheritance;
    public bool AddBackingFields => GenerationSettings.AddBackingFields;
    public string CollectionTypeName => ConstructorSettings.CollectionTypeName;
    public ArgumentValidationType ValidateArguments => ConstructorSettings.ValidateArguments;
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

    public ArgumentValidationType AddValidationCode
    {
        get
        {
            if (ConstructorSettings.ValidateArguments == ArgumentValidationType.None)
            {
                // Do not validate arguments
                return ArgumentValidationType.None;
            }

            if (!InheritanceSettings.EnableInheritance)
            {
                // In case inheritance is enabled, then we want to add validation
                return ConstructorSettings.ValidateArguments;
            }

            if (InheritanceSettings.IsAbstract)
            {
                // Abstract class with base class
                return ArgumentValidationType.None;
            }

            if (InheritanceSettings.BaseClass is null)
            {
                // Abstract base class
                return ArgumentValidationType.None;
            }

            // In other situations, add it
            return ConstructorSettings.ValidateArguments;
        }
    }

    public PipelineSettings(
        PipelineNameSettings? nameSettings = null, 
        PipelineConstructorSettings? constructorSettings = null,
        PipelineInheritanceSettings? inheritanceSettings = null,
        PipelineTypeSettings? typeSettings = null,
        PipelineBuilderCopySettings? copySettings = null,
        PipelineGenerationSettings? generationSettings = null,
        PipelineBuilderNullCheckSettings? nullCheckSettings = null)
    {
        NameSettings = nameSettings ?? new();
        ConstructorSettings = constructorSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        TypeSettings = typeSettings ?? new();
        CopySettings = copySettings ?? new();
        GenerationSettings = generationSettings ?? new();
        NullCheckSettings = nullCheckSettings ?? new();
    }
}
