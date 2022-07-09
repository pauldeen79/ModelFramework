namespace ModelFramework.Objects.Settings;

public record ImmutableClassInheritanceSettings
{
    public bool EnableInheritance { get; }

    public ImmutableClassInheritanceSettings(bool enableInheritance = false)
    {
        EnableInheritance = enableInheritance;
    }
}
