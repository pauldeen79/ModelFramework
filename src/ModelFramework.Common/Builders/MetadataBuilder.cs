using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;

namespace ModelFramework.Common.Builders
{
    public class MetadataBuilder
    {
        public object? Value { get; set; }
        public string Name { get; set; }
        public IMetadata Build()
        {
            return new Metadata(Name, Value);
        }
        public MetadataBuilder Clear()
        {
            Value = default;
            Name = string.Empty;
            return this;
        }
        public MetadataBuilder WithValue(object? value)
        {
            Value = value;
            return this;
        }
        public MetadataBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public MetadataBuilder()
        {
            Name = string.Empty;
        }
        public MetadataBuilder(IMetadata source)
        {
            Value = source.Value;
            Name = source.Name;
        }
    }
}
