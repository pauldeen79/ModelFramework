namespace ClassFramework.Pipelines.Reflection;

public record PipelineBuilderTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineBuilderTypeSettings(
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(string.Empty, false, namespaceMappings, typenameMappings)
    {
    }
}
