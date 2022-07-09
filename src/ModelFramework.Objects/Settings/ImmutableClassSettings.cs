namespace ModelFramework.Objects.Settings;

public record ImmutableClassSettings
{
    public string NewCollectionTypeName { get; }
    public bool CreateWithMethod { get; }
    public bool ImplementIEquatable { get; }
    public bool AddPrivateSetters { get; }
    public ImmutableClassConstructorSettings ConstructorSettings { get; }
    public ImmutableClassInheritanceSettings InheritanceSettings { get; }

    public ImmutableClassSettings(string newCollectionTypeName = "System.Collections.Immutable.IImmutableList",
                                  bool createWithMethod = false,
                                  bool implementIEquatable = false,
                                  bool addPrivateSetters = false,
                                  ImmutableClassConstructorSettings? constructorSettings = null,
                                  ImmutableClassInheritanceSettings? inheritanceSettings = null)
    {
        NewCollectionTypeName = newCollectionTypeName;
        CreateWithMethod = createWithMethod;
        ImplementIEquatable = implementIEquatable;
        AddPrivateSetters = addPrivateSetters;
        ConstructorSettings = constructorSettings ?? new ImmutableClassConstructorSettings();
        InheritanceSettings = inheritanceSettings ?? new ImmutableClassInheritanceSettings();
    }
}
