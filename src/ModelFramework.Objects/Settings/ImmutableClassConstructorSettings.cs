namespace ModelFramework.Objects.Settings;

public class ImmutableClassConstructorSettings
{
    public bool ValidateArguments { get; }
    public bool AddNullChecks { get; }

    public ImmutableClassConstructorSettings(bool validateArguments = false,
                                             bool addNullChecks = false)
    {
        ValidateArguments = validateArguments;
        AddNullChecks = addNullChecks;
    }
}
