namespace ClassFramework.Pipelines.OverrideEntity;

public record OverrideEntityContext : ContextBase<IType, PipelineSettings>
{
    public OverrideEntityContext(IType sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }

    public bool IsAbstract
        => Settings.InheritanceSettings.EnableInheritance
        && Settings.InheritanceSettings.IsAbstract;

    public string MapTypeName(string typeName)
        => typeName.IsNotNull(nameof(typeName)).MapTypeName(Settings.TypeSettings);

    public string MapNamespace(string? ns)
        => ns.MapNamespace(Settings.TypeSettings);
}
