﻿namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassNameSettings
{
    public string SetMethodNameFormatString { get; }
    public string AddMethodNameFormatString { get; }
    public string BuildersNamespace { get; }
    public string BuilderNameFormatString { get; }

    public ImmutableBuilderClassNameSettings(string setMethodNameFormatString = "With{0}",
                                             string addMethodNameFormatString = "Add{0}",
                                             string buildersNamespace = "",
                                             string builderNameFormatString = "{0}Builder")
    {
        SetMethodNameFormatString = setMethodNameFormatString;
        AddMethodNameFormatString = addMethodNameFormatString;
        BuildersNamespace = buildersNamespace;
        BuilderNameFormatString = builderNameFormatString;
    }
}
