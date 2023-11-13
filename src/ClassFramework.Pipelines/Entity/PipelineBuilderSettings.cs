namespace ClassFramework.Pipelines.Entity;

public sealed record PipelineBuilderSettings : IPropertyGenerationSettings
{
    public PipelineBuilderNameSettings NameSettings { get; }
    public PipelineBuilderConstructorSettings ConstructorSettings { get; }
    public PipelineBuilderInheritanceSettings InheritanceSettings { get; }
    public PipelineBuilderTypeSettings TypeSettings { get; }
    public PipelineBuilderGenerationSettings GenerationSettings { get; }

    bool IPropertyGenerationSettings.EnableNullableReferenceTypes => GenerationSettings.EnableNullableReferenceTypes;
    bool IPropertyGenerationSettings.AddNullChecks => GenerationSettings.AddNullChecks;
    ArgumentValidationType IPropertyGenerationSettings.ValidateArguments => ConstructorSettings.ValidateArguments;

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

    public PipelineBuilderSettings(
        PipelineBuilderNameSettings? nameSettings = null, 
        PipelineBuilderConstructorSettings? constructorSettings = null,
        PipelineBuilderInheritanceSettings? inheritanceSettings = null,
        PipelineBuilderTypeSettings? typeSettings = null,
        PipelineBuilderGenerationSettings? generationSettings = null)
    {
        NameSettings = nameSettings ?? new();
        ConstructorSettings = constructorSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        TypeSettings = typeSettings ?? new();
        GenerationSettings = generationSettings ?? new();
    }
}
