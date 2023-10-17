namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderInheritanceSettings
{
    public bool IsAbstract { get; }
    public bool EnableBuilderInheritance { get; }
    public Class? BaseClass { get; }
    public string? BaseClassBuilderNameSpace { get; }
    public Func<IParentTypeContainer, TypeBase, bool>? InheritanceComparisonDelegate { get; }

    public PipelineBuilderInheritanceSettings(
        bool enableBuilderInheritance = false,
        bool isAbstract = false,
        Class? baseClass = null,
        string? baseClassBuilderNameSpace = null,
        Func<IParentTypeContainer, TypeBase, bool>? inheritanceComparisonDelegate = null)
    {
        EnableBuilderInheritance = enableBuilderInheritance;
        IsAbstract = isAbstract;
        BaseClass = baseClass;
        BaseClassBuilderNameSpace = baseClassBuilderNameSpace;
        InheritanceComparisonDelegate = inheritanceComparisonDelegate;
    }
}
