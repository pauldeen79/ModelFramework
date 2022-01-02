using ModelFramework.Objects.Extensions;

namespace ModelFramework.Objects.Builders
{
    public partial class ClassFieldBuilder
    {
        public ClassFieldBuilder WithCustomModifiers(string? customModifiers)
            => ReplaceMetadata(MetadataNames.CustomModifiers, customModifiers);

        public ClassFieldBuilder ReplaceMetadata(string name, object? newValue)
        {
            Metadata.Replace(name, newValue);
            return this;
        }
    }
}
