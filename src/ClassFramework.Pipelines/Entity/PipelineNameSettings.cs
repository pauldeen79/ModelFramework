namespace ClassFramework.Pipelines.Entity;

public record PipelineNameSettings
{
    public string EntityNamespaceFormatString { get; }
    public string EntityNameFormatString { get; }
    public string ToBuilderFormatString { get; }
    public string ToTypedBuilderFormatString { get; }

    public PipelineNameSettings(string entityNamespaceFormatString = "{Namespace}",
                                string entityNameFormatString = "{Class.Name}{EntityNameSuffix}",
                                string toBuilderFormatString = "ToBuilder",
                                string toTypedBuilderFormatString = "ToTypedBuilder")
    {
        EntityNamespaceFormatString = entityNamespaceFormatString.IsNotNull(entityNamespaceFormatString);
        EntityNameFormatString = entityNameFormatString.IsNotNull(entityNameFormatString);
        ToBuilderFormatString = toBuilderFormatString.IsNotNull(toBuilderFormatString);
        ToTypedBuilderFormatString = toTypedBuilderFormatString.IsNotNull(toBuilderFormatString);
    }
}
