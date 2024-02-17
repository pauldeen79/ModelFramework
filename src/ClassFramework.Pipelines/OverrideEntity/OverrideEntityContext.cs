namespace ClassFramework.Pipelines.OverrideEntity;

public class OverrideEntityContext : ContextBase<IType, PipelineSettings>
{
    public OverrideEntityContext(IType sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }

    public bool IsAbstract
        => Settings.InheritanceSettings.EnableInheritance
        && Settings.InheritanceSettings.IsAbstract;
}
