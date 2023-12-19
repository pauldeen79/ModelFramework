namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderNameSettings
{
    public string EntityNamespaceFormatString { get; }
    public string EntityNameFormatString { get; }

    public PipelineBuilderNameSettings(string entityNamespaceFormatString = "{Namespace}",
                                       string entityNameFormatString = "{Class.Name}{EntityNameSuffix}")
    {
        EntityNamespaceFormatString = entityNamespaceFormatString.IsNotNull(entityNamespaceFormatString);
        EntityNameFormatString = entityNameFormatString.IsNotNull(entityNameFormatString);
    }
}
