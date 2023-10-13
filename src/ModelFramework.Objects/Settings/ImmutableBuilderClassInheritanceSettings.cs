namespace ModelFramework.Objects.Settings;

public class ImmutableBuilderClassInheritanceSettings
{
    public bool EnableEntityInheritance { get; }
    public bool IsAbstract { get; }
    public bool EnableBuilderInheritance { get; }
    public bool RemoveDuplicateWithMethods { get; }
    public IClass? BaseClass { get; }
    public string? BaseClassBuilderNameSpace { get; }
    public Func<IParentTypeContainer, INameContainer, ITypeBase, bool>? InheritanceComparisonFunction { get; }

    public ImmutableBuilderClassInheritanceSettings(bool enableEntityInheritance = false,
                                                    bool enableBuilderInheritance = false,
                                                    bool isAbstract = false,
                                                    bool removeDuplicateWithMethods = false,
                                                    IClass? baseClass = null,
                                                    string? baseClassBuilderNameSpace = null,
                                                    Func<IParentTypeContainer, INameContainer, ITypeBase, bool>? inheritanceComparisonFunction = null)
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
