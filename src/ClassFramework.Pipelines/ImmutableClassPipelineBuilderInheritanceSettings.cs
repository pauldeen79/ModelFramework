﻿namespace ClassFramework.Pipelines;

public record ImmutableClassPipelineBuilderInheritanceSettings
{
    public bool EnableInheritance { get; }
    public bool IsAbstract { get; }
    public bool InheritFromInterfaces { get; }
    public Class? BaseClass { get; }
    public Func<string, TypeBase, bool>? InheritanceComparisonFunction { get; }
    public Func<TypeBase, bool, string>? FormatInstanceTypeNameDelegate { get; }

    public ImmutableClassPipelineBuilderInheritanceSettings(
        bool enableInheritance = false,
        bool isAbstract = false,
        bool inheritFromInterfaces = false,
        Class? baseClass = null,
        Func<string, TypeBase, bool>? inheritanceComparisonFunction = null,
        Func<TypeBase, bool, string>? formatInstanceTypeNameDelegate = null)
    {
        EnableInheritance = enableInheritance;
        IsAbstract = isAbstract;
        InheritFromInterfaces = inheritFromInterfaces;
        BaseClass = baseClass;
        InheritanceComparisonFunction = inheritanceComparisonFunction;
        FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
    }
}