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
    public bool AddValidationCode
    {
        get
        {
            if (!ConstructorSettings.ValidateArguments)
            {
                // Do not validate arguments
                return false;
            }

            if (!InheritanceSettings.EnableInheritance)
            {
                // In case inheritance is enabled, then we want to add validation
                return true;
            }

            if (InheritanceSettings.IsAbstract)
            {
                // Abstract class with base class
                return false;
            }

            if (InheritanceSettings.BaseClass == null)
            {
                // Abstract base class
                return false;
            }

            // In other situations, add it
            return true;
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
        ConstructorSettings = constructorSettings ?? new ImmutableClassConstructorSettings();
        InheritanceSettings = inheritanceSettings ?? new ImmutableClassInheritanceSettings();
    }
}
