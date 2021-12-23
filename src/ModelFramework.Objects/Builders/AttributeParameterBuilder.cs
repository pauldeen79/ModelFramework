using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Builders
{
    public class AttributeParameterBuilder
    {
        public object Value { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public string Name { get; set; }
        public IAttributeParameter Build()
        {
            return new AttributeParameter(Value, Name, Metadata.Select(x => x.Build()));
        }
        public AttributeParameterBuilder Clear()
        {
            Value = new object();
            Metadata.Clear();
            Name = string.Empty;
            return this;
        }
        public AttributeParameterBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }
        public AttributeParameterBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public AttributeParameterBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public AttributeParameterBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public AttributeParameterBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public AttributeParameterBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public AttributeParameterBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public AttributeParameterBuilder()
        {
            Name = string.Empty;
            Value = new object();
            Metadata = new List<MetadataBuilder>();
        }
        public AttributeParameterBuilder(IAttributeParameter source)
        {
            Value = source.Value;
            Metadata = new List<MetadataBuilder>(source.Metadata.Select(x => new MetadataBuilder(x)));
            Name = source.Name;
        }
    }
}
