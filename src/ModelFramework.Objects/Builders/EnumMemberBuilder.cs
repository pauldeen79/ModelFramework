using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Builders
{
    public class EnumMemberBuilder
    {
        public List<AttributeBuilder> Attributes { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IEnumMember Build()
        {
            return new EnumMember(Name,
                                  Value,
                                  Attributes.Select(x => x.Build()),
                                  Metadata.Select(x => x.Build()));
        }
        public EnumMemberBuilder Clear()
        {
            Attributes.Clear();
            Name = default;
            Value = default;
            Metadata.Clear();
            return this;
        }
        public EnumMemberBuilder Update(IEnumMember source)
        {
            Attributes = new List<AttributeBuilder>();
            Metadata = new List<MetadataBuilder>();

            Attributes.AddRange(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
            Name = source.Name;
            Value = source.Value;
            Metadata.AddRange(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());

            return this;
        }
        public EnumMemberBuilder ClearAttributes()
        {
            Attributes.Clear();
            return this;
        }
        public EnumMemberBuilder AddAttributes(IEnumerable<AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public EnumMemberBuilder AddAttributes(params AttributeBuilder[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
            return this;
        }
        public EnumMemberBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public EnumMemberBuilder AddAttributes(params IAttribute[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            }
            return this;
        }
        public EnumMemberBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public EnumMemberBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }
        public EnumMemberBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public EnumMemberBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public EnumMemberBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public EnumMemberBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public EnumMemberBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public EnumMemberBuilder()
        {
            Attributes = new List<AttributeBuilder>();
            Metadata = new List<MetadataBuilder>();
        }
        public EnumMemberBuilder(IEnumMember source)
        {
            Attributes = new List<AttributeBuilder>(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
            Name = source.Name;
            Value = source.Value;
            Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
        }
    }
}
