namespace ClassFramework.Pipelines.BuilderInterface;

public class PipelineNameSettings
{
    public string SetMethodNameFormatString { get; }
    public string AddMethodNameFormatString { get; }
    public string BuilderNamespaceFormatString { get; }
    public string BuilderNameFormatString { get; }

    public PipelineNameSettings(string setMethodNameFormatString = "With{Name}",
                                string addMethodNameFormatString = "Add{Name}",
                                string builderNamespaceFormatString = "{Namespace}.Builders",
                                string builderNameFormatString = "{Class.Name}BuilderExtensions")
    {
        SetMethodNameFormatString = setMethodNameFormatString.IsNotNull(setMethodNameFormatString);
        AddMethodNameFormatString = addMethodNameFormatString.IsNotNull(addMethodNameFormatString);
        BuilderNamespaceFormatString = builderNamespaceFormatString.IsNotNull(builderNamespaceFormatString);
        BuilderNameFormatString = builderNameFormatString.IsNotNull(builderNameFormatString);
    }
}
