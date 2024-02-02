namespace ClassFramework.Pipelines.OverrideEntity;

public record PipelineNameSettings
{
    public string EntityNamespaceFormatString { get; }
    public string EntityNameFormatString { get; }

    public PipelineNameSettings(string entityNamespaceFormatString = "{Namespace}",
                                string entityNameFormatString = "{Class.Name}")
    {
        EntityNamespaceFormatString = entityNamespaceFormatString.IsNotNull(entityNamespaceFormatString);
        EntityNameFormatString = entityNameFormatString.IsNotNull(entityNameFormatString);
    }
}
