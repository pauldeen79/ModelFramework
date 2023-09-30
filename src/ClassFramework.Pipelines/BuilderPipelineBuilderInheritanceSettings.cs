namespace ClassFramework.Pipelines;

public record BuilderPipelineBuilderInheritanceSettings
{
    public bool EnableEntityInheritance { get; }
    public bool IsAbstract { get; }
    public bool EnableBuilderInheritance { get; }
    public bool RemoveDuplicateWithMethods { get; }
    public Class? BaseClass { get; }
    public string? BaseClassBuilderNameSpace { get; }
    public Func<string, TypeBase, bool>? InheritanceComparisonFunction { get; }

    public BuilderPipelineBuilderInheritanceSettings(
        bool enableEntityInheritance = false,
        bool enableBuilderInheritance = false,
        bool isAbstract = false,
        bool removeDuplicateWithMethods = false,
        Class? baseClass = null,
        string? baseClassBuilderNameSpace = null,
        Func<string, TypeBase, bool>? inheritanceComparisonFunction = null)
    {
        EnableEntityInheritance = enableEntityInheritance;
        EnableBuilderInheritance = enableBuilderInheritance;
        IsAbstract = isAbstract;
        RemoveDuplicateWithMethods = removeDuplicateWithMethods;
        BaseClass = baseClass;
        BaseClassBuilderNameSpace = baseClassBuilderNameSpace;
        InheritanceComparisonFunction = inheritanceComparisonFunction;
    }
}
