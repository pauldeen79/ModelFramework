using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Builders
{
    public class AttributeBuilder
    {
        public List<AttributeParameterBuilder> Parameters { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public string Name { get; set; }
        public IAttribute Build()
        {
            return new Attribute(Name, Parameters.Select(x => x.Build()), Metadata.Select(x => x.Build()));
        }
        public AttributeBuilder Clear()
        {
            Parameters.Clear();
            Metadata.Clear();
            Name = string.Empty;
            return this;
        }
        public AttributeBuilder ClearParameters()
        {
            Parameters.Clear();
            return this;
        }
        public AttributeBuilder AddParameters(IEnumerable<AttributeParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }
        public AttributeBuilder AddParameters(params AttributeParameterBuilder[] parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }
        public AttributeBuilder AddParameters(IEnumerable<IAttributeParameter> parameters)
        {
            return AddParameters(parameters.ToArray());
        }
        public AttributeBuilder AddParameters(params IAttributeParameter[] parameters)
        {
            Parameters.AddRange(parameters.Select(x => new AttributeParameterBuilder(x)));
            return this;
        }
        public AttributeBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public AttributeBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public AttributeBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public AttributeBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public AttributeBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public AttributeBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public AttributeBuilder()
        {
            Name = string.Empty;
            Parameters = new List<AttributeParameterBuilder>();
            Metadata = new List<MetadataBuilder>();
        }
        public AttributeBuilder(IAttribute source)
        {
            Parameters = new List<AttributeParameterBuilder>(source.Parameters.Select(x => new AttributeParameterBuilder(x)));
            Metadata = new List<MetadataBuilder>(source.Metadata.Select(x => new MetadataBuilder(x)));
            Name = source.Name;
        }
    }
}
