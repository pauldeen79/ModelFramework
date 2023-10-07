namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderInheritanceSettings
{
    public bool EnableEntityInheritance { get; }
    public bool IsAbstract { get; }
    public bool EnableBuilderInheritance { get; }
    public Class? BaseClass { get; }
    public string? BaseClassBuilderNameSpace { get; }
    public Func<IParentTypeContainer, TypeBase, bool>? InheritanceComparisonFunction { get; }

    public PipelineBuilderInheritanceSettings(
        bool enableEntityInheritance = false,
        bool enableBuilderInheritance = false,
        bool isAbstract = false,
        Class? baseClass = null,
        string? baseClassBuilderNameSpace = null,
        Func<IParentTypeContainer, TypeBase, bool>? inheritanceComparisonFunction = null)
    {
        EnableEntityInheritance = enableEntityInheritance;
        EnableBuilderInheritance = enableBuilderInheritance;
        IsAbstract = isAbstract;
        BaseClass = baseClass;
        BaseClassBuilderNameSpace = baseClassBuilderNameSpace;
        InheritanceComparisonFunction = inheritanceComparisonFunction;
    }
}
