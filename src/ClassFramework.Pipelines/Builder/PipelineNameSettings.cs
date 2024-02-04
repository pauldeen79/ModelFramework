namespace ClassFramework.Pipelines.Builder;

public class PipelineNameSettings
{
    public string SetMethodNameFormatString { get; }
    public string AddMethodNameFormatString { get; }
    public string BuilderNamespaceFormatString { get; }
    public string BuilderNameFormatString { get; }
    public string BuildMethodName { get; }
    public string BuildTypedMethodName { get; }
    public string SetDefaultValuesMethodName { get; }

    public PipelineNameSettings(string setMethodNameFormatString = "With{Name}",
                                string addMethodNameFormatString = "Add{Name}",
                                string builderNamespaceFormatString = "{Namespace}.Builders",
                                string builderNameFormatString = "{Class.Name}Builder",
                                string buildMethodName = "Build",
                                string buildTypedMethodName = "BuildTyped",
                                string setDefaultValuesMethodName = "SetDefaultValues")
    {
        SetMethodNameFormatString = setMethodNameFormatString.IsNotNull(setMethodNameFormatString);
        AddMethodNameFormatString = addMethodNameFormatString.IsNotNull(addMethodNameFormatString);
        BuilderNamespaceFormatString = builderNamespaceFormatString.IsNotNull(builderNamespaceFormatString);
        BuilderNameFormatString = builderNameFormatString.IsNotNull(builderNameFormatString);
        BuildMethodName = buildMethodName.IsNotNull(buildMethodName);
        BuildTypedMethodName = buildTypedMethodName.IsNotNull(buildTypedMethodName);
        SetDefaultValuesMethodName = setDefaultValuesMethodName.IsNotNull(setDefaultValuesMethodName);
    }
}
