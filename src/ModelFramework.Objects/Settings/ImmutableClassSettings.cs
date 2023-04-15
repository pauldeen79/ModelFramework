namespace ModelFramework.Objects.Settings;

public record ImmutableClassSettings
{
    public string NewCollectionTypeName { get; }
    public bool CreateWithMethod { get; }
    public bool ImplementIEquatable { get; }
    public bool AddPrivateSetters { get; }
    public bool AllowGenerationWithoutProperties { get; }
    public ImmutableClassConstructorSettings ConstructorSettings { get; }
    public ImmutableClassInheritanceSettings InheritanceSettings { get; }
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

            if (InheritanceSettings.BaseClass == null)
            {
                // Abstract base class
                return ArgumentValidationType.None;
            }

            // In other situations, add it
            return ConstructorSettings.ValidateArguments;
        }
    }

    public ImmutableClassSettings(string newCollectionTypeName = "System.Collections.Immutable.IImmutableList",
                                  bool createWithMethod = false,
                                  bool implementIEquatable = false,
                                  bool addPrivateSetters = false,
                                  bool allowGenerationWithoutProperties = false,
                                  ImmutableClassConstructorSettings? constructorSettings = null,
                                  ImmutableClassInheritanceSettings? inheritanceSettings = null)
    {
        NewCollectionTypeName = newCollectionTypeName;
        CreateWithMethod = createWithMethod;
        ImplementIEquatable = implementIEquatable;
        AddPrivateSetters = addPrivateSetters;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        ConstructorSettings = constructorSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
    }
}
