namespace ClassFramework.Pipelines.Reflection;

public record PipelineBuilderNameSettings
{
    public string NamespaceFormatString { get; }
    public string NameFormatString { get; }

    public PipelineBuilderNameSettings(string namespaceFormatString = "{Namespace}",
                                       string nameFormatString = "{Class.Name}")
    {
        NamespaceFormatString = namespaceFormatString.IsNotNull(namespaceFormatString);
        NameFormatString = nameFormatString.IsNotNull(nameFormatString);
    }
}
