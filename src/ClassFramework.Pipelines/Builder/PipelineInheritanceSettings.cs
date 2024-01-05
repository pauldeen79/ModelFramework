namespace ClassFramework.Pipelines.Builder;

public record PipelineInheritanceSettings
{
    public bool IsAbstract { get; }
    public bool EnableBuilderInheritance { get; }
    public Class? BaseClass { get; }
    public string? BaseClassBuilderNameSpace { get; }
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate { get; }

    public PipelineInheritanceSettings(
        bool enableBuilderInheritance = false,
        bool isAbstract = false,
        Class? baseClass = null,
        string? baseClassBuilderNameSpace = null,
        Func<IParentTypeContainer, IType, bool>? inheritanceComparisonDelegate = null)
    {
        EnableBuilderInheritance = enableBuilderInheritance;
        IsAbstract = isAbstract;
        BaseClass = baseClass;
        BaseClassBuilderNameSpace = baseClassBuilderNameSpace;
        InheritanceComparisonDelegate = inheritanceComparisonDelegate;
    }
}
