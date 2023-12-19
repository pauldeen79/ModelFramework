namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderInheritanceSettings
{
    public bool EnableInheritance { get; }
    public bool IsAbstract { get; }
    public Class? BaseClass { get; }
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate { get; }

    public PipelineBuilderInheritanceSettings(
        bool enableInheritance = false,
        bool isAbstract = false,
        Class? baseClass = null,
        Func<IParentTypeContainer, IType, bool>? inheritanceComparisonDelegate = null)
    {
        EnableInheritance = enableInheritance;
        IsAbstract = isAbstract;
        BaseClass = baseClass;
        InheritanceComparisonDelegate = inheritanceComparisonDelegate;
    }
}
