namespace ClassFramework.Pipelines.Interface;

public record PipelineNameSettings
{
    public string NamespaceFormatString { get; }
    public string NameFormatString { get; }

    public PipelineNameSettings(string namespaceFormatString = "{Namespace}",
                                string nameFormatString = "{Class.Name}")
    {
        NamespaceFormatString = namespaceFormatString.IsNotNull(namespaceFormatString);
        NameFormatString = nameFormatString.IsNotNull(nameFormatString);
    }
}
