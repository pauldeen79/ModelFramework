namespace ModelFramework.Objects.Settings;

public record ImmutableClassSettings
{
    public string NewCollectionTypeName { get; }
    public bool CreateWithMethod { get; }
    public bool ImplementIEquatable { get; }
    public bool ValidateArgumentsInConstructor { get; }
    public bool AddNullChecks { get; }
    public bool AddPrivateSetters { get; }

    public ImmutableClassSettings(string newCollectionTypeName = "System.Collections.Immutable.IImmutableList",
                                  bool createWithMethod = false,
                                  bool implementIEquatable = false,
                                  bool validateArgumentsInConstructor = false,
                                  bool addNullChecks = false,
                                  bool addPrivateSetters = false)
    {
        NewCollectionTypeName = newCollectionTypeName;
        CreateWithMethod = createWithMethod;
        ImplementIEquatable = implementIEquatable;
        ValidateArgumentsInConstructor = validateArgumentsInConstructor;
        AddNullChecks = addNullChecks;
        AddPrivateSetters = addPrivateSetters;
    }
}
