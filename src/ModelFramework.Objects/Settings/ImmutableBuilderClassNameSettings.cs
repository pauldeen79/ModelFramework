namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassNameSettings
{
    public string SetMethodNameFormatString { get; }
    public string AddMethodNameFormatString { get; }

    public ImmutableBuilderClassNameSettings(string setMethodNameFormatString = "With{0}",
                                             string addMethodNameFormatString = "Add{0}")
    {
        SetMethodNameFormatString = setMethodNameFormatString;
        AddMethodNameFormatString = addMethodNameFormatString;
    }
}
