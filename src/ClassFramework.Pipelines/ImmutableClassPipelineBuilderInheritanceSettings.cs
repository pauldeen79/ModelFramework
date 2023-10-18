﻿namespace ClassFramework.Pipelines;

public record ImmutableClassPipelineBuilderInheritanceSettings
{
    public bool EnableInheritance { get; }
    public bool IsAbstract { get; }
    public bool InheritFromInterfaces { get; }
    public Class? BaseClass { get; }
    public Func<string, TypeBase, bool>? InheritanceComparisonDelegate { get; }

    public ImmutableClassPipelineBuilderInheritanceSettings(
        bool enableInheritance = false,
        bool isAbstract = false,
        bool inheritFromInterfaces = false,
        Class? baseClass = null,
        Func<string, TypeBase, bool>? inheritanceComparisonDelegate = null)
    {
        EnableInheritance = enableInheritance;
        IsAbstract = isAbstract;
        InheritFromInterfaces = inheritFromInterfaces;
        BaseClass = baseClass;
        InheritanceComparisonDelegate = inheritanceComparisonDelegate;
    }
}
