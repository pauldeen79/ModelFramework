namespace ModelFramework.Objects.Settings;

public enum ArgumentValidationType
{
    None,
    Shared,
    DomainOnly
}

public class ImmutableClassConstructorSettings
{
    public ArgumentValidationType ValidateArguments { get; }
    public ArgumentValidationType OriginalValidateArguments { get; }
    public bool AddNullChecks { get; }
    public string CollectionTypeName { get; }

    public ImmutableClassConstructorSettings(ArgumentValidationType validateArguments = ArgumentValidationType.None,
                                             ArgumentValidationType? originalValidateArguments = null,
                                             bool addNullChecks = false,
                                             string collectionTypeName = "")
    {
        ValidateArguments = validateArguments;
        OriginalValidateArguments = originalValidateArguments ?? validateArguments;
        AddNullChecks = addNullChecks;
        CollectionTypeName = collectionTypeName;
    }
}
