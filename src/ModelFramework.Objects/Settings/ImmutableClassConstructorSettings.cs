namespace ModelFramework.Objects.Settings;

public class ImmutableClassConstructorSettings
{
    public bool ValidateArguments { get; }
    public bool AddNullChecks { get; }
    public string CollectionTypeName { get; }

    public ImmutableClassConstructorSettings(bool validateArguments = false,
                                             bool addNullChecks = false,
                                             string collectionTypeName = "")
    {
        ValidateArguments = validateArguments;
        AddNullChecks = addNullChecks;
        CollectionTypeName = collectionTypeName;
    }
}
