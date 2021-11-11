using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;

namespace ModelFramework.Common.Builders
{
    public class MetadataBuilder
    {
        public object Value { get; set; }
        public string Name { get; set; }
        public IMetadata Build()
        {
            return new Metadata(Name, Value);
        }
        public MetadataBuilder Clear()
        {
            Value = default;
            Name = default;
            return this;
        }
        public MetadataBuilder Update(IMetadata source)
        {
            Value = default;
            Name = default;
            if (source != null)
            {
                Value = source.Value;
                Name = source.Name;
            }
            return this;
        }
        public MetadataBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }
        public MetadataBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public MetadataBuilder(IMetadata source = null)
        {
            if (source != null)
            {
                Value = source.Value;
                Name = source.Name;
            }
        }
        public MetadataBuilder(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
