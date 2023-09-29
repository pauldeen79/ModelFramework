namespace ClassFramework.Pipelines;

public record ImmutableClassPipelineBuilderInheritanceSettings
{
    public bool EnableInheritance { get; }
    public bool IsAbstract { get; }
    public bool InheritFromInterfaces { get; }
    public Class? BaseClass { get; }
    public Func<string, Domain.Type, bool>? InheritanceComparisonFunction { get; }
    public Func<Domain.Type, bool, string>? FormatInstanceTypeNameDelegate { get; }

    public ImmutableClassPipelineBuilderInheritanceSettings(
        bool enableInheritance = false,
        bool isAbstract = false,
        bool inheritFromInterfaces = false,
        Class? baseClass = null,
        Func<string, Domain.Type, bool>? inheritanceComparisonFunction = null,
        Func<Domain.Type, bool, string>? formatInstanceTypeNameDelegate = null)
    {
        EnableInheritance = enableInheritance;
        IsAbstract = isAbstract;
        InheritFromInterfaces = inheritFromInterfaces;
        BaseClass = baseClass;
        InheritanceComparisonFunction = inheritanceComparisonFunction;
        FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
    }
}
