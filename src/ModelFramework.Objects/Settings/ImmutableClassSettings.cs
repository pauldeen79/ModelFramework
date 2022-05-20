namespace ModelFramework.Objects.Settings;

public record ImmutableClassSettings
{
    public string NewCollectionTypeName { get; }
    public bool CreateWithMethod { get; }
    public bool ImplementIEquatable { get; }
    public bool ValidateArgumentsInConstructor { get; }
    public bool AddNullChecks { get; }

    public ImmutableClassSettings(string newCollectionTypeName = "System.Collections.Immutable.IImmutableList",
                                  bool createWithMethod = false,
                                  bool implementIEquatable = false,
                                  bool validateArgumentsInConstructor = false,
                                  bool addNullChecks = false)
    {
        NewCollectionTypeName = newCollectionTypeName;
        CreateWithMethod = createWithMethod;
        ImplementIEquatable = implementIEquatable;
        ValidateArgumentsInConstructor = validateArgumentsInConstructor;
        AddNullChecks = addNullChecks;
    }
}
