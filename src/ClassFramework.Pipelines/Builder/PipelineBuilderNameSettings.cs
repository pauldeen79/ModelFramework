namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderNameSettings
{
    public string SetMethodNameFormatString { get; }
    public string AddMethodNameFormatString { get; }
    public string BuilderNamespaceFormatString { get; }
    public string BuilderNameFormatString { get; }
    public string BuildMethodName { get; }
    public string BuildTypedMethodName { get; }

    public PipelineBuilderNameSettings(string setMethodNameFormatString = "With{Name}",
                                              string addMethodNameFormatString = "Add{Name}",
                                              string builderNamespaceFormatString = "{Namespace}",
                                              string builderNameFormatString = "{Name}Builder",
                                              string buildMethodName = "Build",
                                              string buildTypedMethodName = "BuildTyped")
    {
        SetMethodNameFormatString = setMethodNameFormatString.IsNotNull(setMethodNameFormatString);
        AddMethodNameFormatString = addMethodNameFormatString.IsNotNull(addMethodNameFormatString);
        BuilderNamespaceFormatString = builderNamespaceFormatString.IsNotNull(builderNamespaceFormatString);
        BuilderNameFormatString = builderNameFormatString.IsNotNull(builderNameFormatString);
        BuildMethodName = buildMethodName.IsNotNull(buildMethodName);
        BuildTypedMethodName = buildTypedMethodName.IsNotNull(buildTypedMethodName);
    }
}
