namespace ClassFramework.Pipelines.BuilderInterface;

public class PipelineInheritanceSettings
{
    public bool IsAbstract { get; }
    public bool EnableBuilderInheritance { get; }
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate { get; }

    public PipelineInheritanceSettings(
        bool enableBuilderInheritance = false,
        bool isAbstract = false,
        Func<IParentTypeContainer, IType, bool>? inheritanceComparisonDelegate = null)
    {
        EnableBuilderInheritance = enableBuilderInheritance;
        IsAbstract = isAbstract;
        InheritanceComparisonDelegate = inheritanceComparisonDelegate;
    }
}
