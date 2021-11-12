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
        public AttributeBuilder Update(IAttribute source)
        {
            Parameters = new List<AttributeParameterBuilder>();
            Metadata = new List<MetadataBuilder>();
            Name = default;
            if (source != null)
            {
                if (source.Parameters != null) Parameters.AddRange(source.Parameters.Select(x => new AttributeParameterBuilder(x)));
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                Name = source.Name;
            }
            return this;
        }
        public AttributeBuilder Clear()
        {
            Parameters.Clear();
            Metadata.Clear();
            Name = default;
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
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public AttributeBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public AttributeBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public AttributeBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public AttributeBuilder(IAttribute source = null)
        {
            if (source != null)
            {
                if (source.Parameters != null) Parameters = new List<AttributeParameterBuilder>(source.Parameters?.Select(x => new AttributeParameterBuilder(x)) ?? Enumerable.Empty<AttributeParameterBuilder>());
                if (source.Metadata != null) Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
                Name = source.Name;
            }
            else
            {
                Parameters = new List<AttributeParameterBuilder>();
                Metadata = new List<MetadataBuilder>();
            }
        }
        public AttributeBuilder(string name, IEnumerable<IAttributeParameter> parameters = null, IEnumerable<IMetadata> metadata = null)
        {
            Parameters = new List<AttributeParameterBuilder>();
            Metadata = new List<MetadataBuilder>();
            Name = name;
            if (parameters != null) Parameters.AddRange(parameters.Select(x => new AttributeParameterBuilder(x)));
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
