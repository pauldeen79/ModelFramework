namespace ClassFramework.Pipelines.Reflection;

public record PipelineTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineTypeSettings(
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(string.Empty, false, namespaceMappings, typenameMappings)
    {
    }
}
