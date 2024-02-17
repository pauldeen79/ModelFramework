namespace ClassFramework.Pipelines.Entity;

public class EntityContext : ContextBase<IType, PipelineSettings>
{
    public EntityContext(IType sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }

    public bool IsAbstract
        => Settings.InheritanceSettings.EnableInheritance
        && Settings.InheritanceSettings.IsAbstract;
}
