using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Builders
{
    public class ParameterBuilder
    {
        public string TypeName { get; set; }
        public List<AttributeBuilder> Attributes { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public string Name { get; set; }
        public object DefaultValue { get; set; }
        public bool IsNullable { get; set; }
        public IParameter Build()
        {
            return new Parameter(Name,
                                 TypeName,
                                 DefaultValue,
                                 IsNullable,
                                 Attributes.Select(x => x.Build()), Metadata.Select(x => x.Build()));
        }
        public ParameterBuilder Clear()
        {
            TypeName = default;
            Attributes.Clear();
            Metadata.Clear();
            Name = default;
            DefaultValue = default;
            IsNullable = default;
            return this;
        }
        public ParameterBuilder Update(IParameter source)
        {
            TypeName = default;
            Attributes = new List<AttributeBuilder>();
            Metadata = new List<MetadataBuilder>();
            Name = default;
            DefaultValue = default;
            if (source != null)
            {
                TypeName = source.TypeName;
                if (source.Attributes != null) Attributes.AddRange(source.Attributes.Select(x => new AttributeBuilder(x)));
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                Name = source.Name;
                DefaultValue = source.DefaultValue;
            }
            return this;
        }
        public ParameterBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
            return this;
        }
        public ParameterBuilder ClearAttributes()
        {
            Attributes.Clear();
            return this;
        }
        public ParameterBuilder AddAttributes(IEnumerable<AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ParameterBuilder AddAttributes(params AttributeBuilder[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
            return this;
        }
        public ParameterBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ParameterBuilder AddAttributes(params IAttribute[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            }
            return this;
        }
        public ParameterBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ParameterBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ParameterBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public ParameterBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ParameterBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ParameterBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ParameterBuilder WithDefaultValue(object defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
        public ParameterBuilder WithIsNullable(bool isNullable)
        {
            IsNullable = isNullable;
            return this;
        }
        public ParameterBuilder(IParameter source = null)
        {
            if (source != null)
            {
                TypeName = source.TypeName;
                Attributes = new List<AttributeBuilder>(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
                Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
                Name = source.Name;
                DefaultValue = source.DefaultValue;
                IsNullable = source.IsNullable;
            }
            else
            {
                Attributes = new List<AttributeBuilder>();
                Metadata = new List<MetadataBuilder>();
            }
        }
        public ParameterBuilder(string name,
                                string typeName,
                                object defaultValue = null,
                                bool isNullable = false,
                                IEnumerable<IAttribute> attributes = null,
                                IEnumerable<IMetadata> metadata = null)
        {
            Attributes = new List<AttributeBuilder>();
            Metadata = new List<MetadataBuilder>();
            Name = name;
            TypeName = typeName;
            DefaultValue = defaultValue;
            IsNullable = isNullable;
            if (attributes != null) Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
