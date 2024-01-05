namespace ClassFramework.Pipelines.Entity;

public record PipelineInheritanceSettings
{
    public bool EnableInheritance { get; }
    public bool IsAbstract { get; }
    public Class? BaseClass { get; }
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate { get; }

    public PipelineInheritanceSettings(
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
