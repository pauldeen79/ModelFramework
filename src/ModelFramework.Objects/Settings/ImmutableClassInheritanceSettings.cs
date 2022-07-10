namespace ModelFramework.Objects.Settings;

public record ImmutableClassInheritanceSettings
{
    public bool EnableInheritance { get; }
    public IClass? BaseClass { get; }

    public ImmutableClassInheritanceSettings(bool enableInheritance = false, IClass? baseClass = null)
    {
        EnableInheritance = enableInheritance;
        BaseClass = baseClass;
    }
}
