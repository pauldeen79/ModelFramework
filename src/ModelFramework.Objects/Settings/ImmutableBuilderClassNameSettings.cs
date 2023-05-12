namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassNameSettings
{
    public string SetMethodNameFormatString { get; }
    public string AddMethodNameFormatString { get; }
    public string BuildersNamespace { get; }
    public string BuilderNameFormatString { get; }
    public string BuildMethodName { get; }
    public string BuildTypedMethodName { get; }

    public ImmutableBuilderClassNameSettings(string setMethodNameFormatString = "With{0}",
                                             string addMethodNameFormatString = "Add{0}",
                                             string buildersNamespace = "",
                                             string builderNameFormatString = "{0}Builder",
                                             string buildMethodName = "Build",
                                             string buildTypedMethodName = "BuildTyped")
    {
        SetMethodNameFormatString = setMethodNameFormatString;
        AddMethodNameFormatString = addMethodNameFormatString;
        BuildersNamespace = buildersNamespace;
        BuilderNameFormatString = builderNameFormatString;
        BuildMethodName = buildMethodName;
        BuildTypedMethodName = buildTypedMethodName;
    }
}
