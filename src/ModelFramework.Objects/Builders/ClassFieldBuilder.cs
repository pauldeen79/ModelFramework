namespace ModelFramework.Objects.Builders;

public partial class ClassFieldBuilder
{
    public ClassFieldBuilder WithCustomModifiers(string? customModifiers)
        => ReplaceMetadata(MetadataNames.CustomModifiers, customModifiers);

    public ClassFieldBuilder ReplaceMetadata(string name, object? newValue)
        => this.Chain(() => Metadata.Replace(name, newValue));
}
