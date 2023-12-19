namespace ModelFramework.Objects.Settings;

public record ImmutableClassInheritanceSettings
{
    public bool EnableInheritance { get; }
    public bool IsAbstract { get; }
    public bool InheritFromInterfaces { get; }
    public IClass? BaseClass { get; }
    public Func<IParentTypeContainer, INameContainer, ITypeBase, bool>? InheritanceComparisonDelegate { get; }
    public Func<ITypeBase, bool, string>? FormatInstanceTypeNameDelegate { get; }

    public ImmutableClassInheritanceSettings(bool enableInheritance = false,
                                             bool isAbstract = false,
                                             bool inheritFromInterfaces = false,
                                             IClass? baseClass = null,
                                             Func<IParentTypeContainer, INameContainer, ITypeBase, bool>? inheritanceComparisonDelegate = null,
                                             Func<ITypeBase, bool, string>? formatInstanceTypeNameDelegate = null)
    {
        EnableInheritance = enableInheritance;
        IsAbstract = isAbstract;
        InheritFromInterfaces = inheritFromInterfaces;
        BaseClass = baseClass;
        InheritanceComparisonDelegate = inheritanceComparisonDelegate;
        FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
    }
}
