namespace ClassFramework.Pipelines.Reflection;

public class PipelineInheritanceSettings
{
    public bool EnableInheritance { get; }
    public bool IsAbstract { get; }
    public Class? BaseClass { get; }
    public Func<IParentTypeContainer, Type, bool>? InheritanceComparisonDelegate { get; }

    public PipelineInheritanceSettings(
        bool enableInheritance = false,
        bool isAbstract = false,
        Class? baseClass = null,
        Func<IParentTypeContainer, Type, bool>? inheritanceComparisonDelegate = null)
    {
        EnableInheritance = enableInheritance;
        IsAbstract = isAbstract;
        BaseClass = baseClass;
        InheritanceComparisonDelegate = inheritanceComparisonDelegate;
    }
}
