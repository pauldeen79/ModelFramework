namespace ModelFramework.Objects.Settings;

public class ImmutableBuilderClassInheritanceSettings
{
    public bool EnableInheritance { get; }
    public bool IsAbstract { get; }
    public IClass? BaseClass { get; }
    public Func<IParentTypeContainer, ITypeBase, bool>? InheritanceComparisonFunction { get; }

    public ImmutableBuilderClassInheritanceSettings(bool enableInheritance = false,
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
