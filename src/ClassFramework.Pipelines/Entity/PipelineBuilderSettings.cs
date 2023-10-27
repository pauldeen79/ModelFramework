namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderSettings
{
    public string NewCollectionTypeName { get; }
    public bool AddPrivateSetters { get; }
    public bool EnableNullableReferenceTypes { get; }
    public bool AllowGenerationWithoutProperties { get; }
    public PipelineBuilderNameSettings NameSettings { get; }
    public PipelineBuilderConstructorSettings ConstructorSettings { get; }
    public PipelineBuilderInheritanceSettings InheritanceSettings { get; }
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
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        bool addPrivateSetters = false,
        bool enableNullableReferenceTypes = false,
        bool allowGenerationWithoutProperties = false,
        PipelineBuilderNameSettings? nameSettings = null, 
        PipelineBuilderConstructorSettings? constructorSettings = null,
        PipelineBuilderInheritanceSettings? inheritanceSettings = null)
    {
        NewCollectionTypeName = newCollectionTypeName.IsNotNull(nameof(newCollectionTypeName));
        AddPrivateSetters = addPrivateSetters;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        NameSettings = nameSettings ?? new();
        ConstructorSettings = constructorSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
    }
}
