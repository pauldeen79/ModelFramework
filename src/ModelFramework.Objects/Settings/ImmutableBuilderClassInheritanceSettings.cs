namespace ModelFramework.Objects.Settings;

public class ImmutableBuilderClassInheritanceSettings
{
    public bool EnableInheritance { get; }
    public IClass? BaseClass { get; }

    public ImmutableBuilderClassInheritanceSettings(bool enableInheritance = false, IClass? baseClass = null)
    {
        EnableInheritance = enableInheritance;
        BaseClass = baseClass;
    }
}
