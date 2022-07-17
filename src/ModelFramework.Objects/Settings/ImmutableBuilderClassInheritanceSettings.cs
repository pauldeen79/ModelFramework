namespace ModelFramework.Objects.Settings;

public class ImmutableBuilderClassInheritanceSettings
{
    public bool EnableInheritance { get; }
    public IClass? BaseClass { get; }
    public Func<IParentTypeContainer, ITypeBase, bool>? InheritanceComparisonFunction { get; }

    public ImmutableBuilderClassInheritanceSettings(bool enableInheritance = false,
                                                    IClass? baseClass = null,
                                                    Func<IParentTypeContainer, ITypeBase, bool>? inheritanceComparisonFunction = null)
    {
        EnableInheritance = enableInheritance;
        BaseClass = baseClass;
        InheritanceComparisonFunction = inheritanceComparisonFunction;
    }
}
