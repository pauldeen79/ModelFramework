namespace ClassFramework.Pipelines;

public record ImmutableClassPipelineBuilderSettings
{
    public string NewCollectionTypeName { get; }
    public bool CreateWithMethod { get; }
    public bool ImplementIEquatable { get; }
    public bool AddPrivateSetters { get; }
    public bool AllowGenerationWithoutProperties { get; }
    public ImmutableClassPipelineBuilderConstructorSettings ConstructorSettings { get; }
    public ImmutableClassPipelineBuilderInheritanceSettings InheritanceSettings { get; }
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

    public ImmutableClassPipelineBuilderSettings(
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        bool createWithMethod = false,
        bool implementIEquatable = false,
        bool addPrivateSetters = false,
        bool allowGenerationWithoutProperties = false,
        ImmutableClassPipelineBuilderConstructorSettings? constructorSettings = null,
        ImmutableClassPipelineBuilderInheritanceSettings? inheritanceSettings = null)
    {
        NewCollectionTypeName = newCollectionTypeName.IsNotNull(nameof(newCollectionTypeName));
        CreateWithMethod = createWithMethod;
        ImplementIEquatable = implementIEquatable;
        AddPrivateSetters = addPrivateSetters;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        ConstructorSettings = constructorSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
    }
}
