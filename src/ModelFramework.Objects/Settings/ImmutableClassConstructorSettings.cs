namespace ModelFramework.Objects.Settings;

public enum ArgumentValidationType
{
    Never,
    Optional,
    Always
}

public class ImmutableClassConstructorSettings
{
    public ArgumentValidationType ValidateArguments { get; }
    public bool AddNullChecks { get; }
    public string CollectionTypeName { get; }

    public ImmutableClassConstructorSettings(ArgumentValidationType validateArguments = ArgumentValidationType.Never,
                                             bool addNullChecks = false,
                                             string collectionTypeName = "")
    {
        ValidateArguments = validateArguments;
        AddNullChecks = addNullChecks;
        CollectionTypeName = collectionTypeName;
    }
}
