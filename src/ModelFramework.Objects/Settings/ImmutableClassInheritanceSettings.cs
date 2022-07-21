namespace ModelFramework.Objects.Settings;

public record ImmutableClassInheritanceSettings
{
    public bool EnableInheritance { get; }
    public bool IsAbstract { get; }
    public IClass? BaseClass { get; }
    public Func<IParentTypeContainer, ITypeBase, bool>? InheritanceComparisonFunction { get; }

    public ImmutableClassInheritanceSettings(bool enableInheritance = false,
                                             bool isAbstract = false,
                                             IClass? baseClass = null,
                                             Func<IParentTypeContainer, ITypeBase, bool>? inheritanceComparisonFunction = null)
    {
        EnableInheritance = enableInheritance;
        IsAbstract = isAbstract;
        BaseClass = baseClass;
        InheritanceComparisonFunction = inheritanceComparisonFunction;
    }
}
