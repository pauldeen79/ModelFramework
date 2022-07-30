namespace ModelFramework.Objects.Settings;

public class ImmutableBuilderClassInheritanceSettings
{
    public bool EnableEntityInheritance { get; }
    public bool IsAbstract { get; }
    public bool EnableBuilderInheritance { get; }
    public IClass? BaseClass { get; }
    public Func<IParentTypeContainer, ITypeBase, bool>? InheritanceComparisonFunction { get; }

    public ImmutableBuilderClassInheritanceSettings(bool enableEntityInheritance = false,
                                                    bool enableBuilderInheritance = false,
                                                    bool isAbstract = false,
                                                    IClass? baseClass = null,
                                                    Func<IParentTypeContainer, ITypeBase, bool>? inheritanceComparisonFunction = null)
    {
        EnableEntityInheritance = enableEntityInheritance;
        EnableBuilderInheritance = enableBuilderInheritance;
        IsAbstract = isAbstract;
        BaseClass = baseClass;
        InheritanceComparisonFunction = inheritanceComparisonFunction;
    }
}
