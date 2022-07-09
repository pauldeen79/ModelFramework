namespace ModelFramework.Objects.Settings;

public class ImmutableBuilderClassInheritanceSettings
{
    public bool EnableInheritance { get; }

    public ImmutableBuilderClassInheritanceSettings(bool enableInheritance = false)
    {
        EnableInheritance = enableInheritance;
    }
}
