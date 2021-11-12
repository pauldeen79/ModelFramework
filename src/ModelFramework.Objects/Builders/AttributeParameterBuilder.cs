﻿using System.Collections.Generic;
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
            Value = default;
            Metadata.Clear();
            Name = default;
            return this;
        }
        public AttributeParameterBuilder Update(IAttributeParameter source)
        {
            Value = default;
            Metadata = new List<MetadataBuilder>();
            Name = default;
            if (source != null)
            {
                Value = source.Value;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                Name = source.Name;
            }
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
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public AttributeParameterBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public AttributeParameterBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public AttributeParameterBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public AttributeParameterBuilder(IAttributeParameter source = null)
        {
            if (source != null)
            {
                Value = source.Value;
                if (source.Metadata != null) Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
                Name = source.Name;
            }
            else
            {
                Metadata = new List<MetadataBuilder>();
            }
        }
        public AttributeParameterBuilder(object value, string name = null, IEnumerable<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            Name = name;
            Value = value;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
