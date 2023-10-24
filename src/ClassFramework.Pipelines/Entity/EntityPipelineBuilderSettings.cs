namespace ClassFramework.Pipelines.Entity;

public record EntityPipelineBuilderSettings
{
    public string NewCollectionTypeName { get; }
    public bool AddPrivateSetters { get; }
    public bool EnableNullableReferenceTypes { get; }
    public bool AllowGenerationWithoutProperties { get; }
    public EntityPipelineBuilderConstructorSettings ConstructorSettings { get; }
    public EntityPipelineBuilderInheritanceSettings InheritanceSettings { get; }
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

    public EntityPipelineBuilderSettings(
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        bool addPrivateSetters = false,
        bool enableNullableReferenceTypes = false,
        bool allowGenerationWithoutProperties = false,
        EntityPipelineBuilderConstructorSettings? constructorSettings = null,
        EntityPipelineBuilderInheritanceSettings? inheritanceSettings = null)
    {
        NewCollectionTypeName = newCollectionTypeName.IsNotNull(nameof(newCollectionTypeName));
        AddPrivateSetters = addPrivateSetters;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        ConstructorSettings = constructorSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
    }
}
