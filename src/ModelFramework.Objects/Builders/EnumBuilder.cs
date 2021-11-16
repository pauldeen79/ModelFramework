using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Builders
{
    public class EnumBuilder
    {
        public List<AttributeBuilder> Attributes { get; set; }
        public List<EnumMemberBuilder> Members { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public Visibility Visibility { get; set; }
        public IEnum Build()
        {
            return new Enum(Name,
                            Members.Select(x => x.Build()),
                            Visibility,
                            Attributes.Select(x => x.Build()),
                            Metadata.Select(x => x.Build()));
        }
        public EnumBuilder Clear()
        {
            Attributes.Clear();
            Members.Clear();
            Name = default;
            Metadata.Clear();
            Visibility = default;
            return this;
        }
        public EnumBuilder Update(IEnum source)
        {
            Attributes = new List<AttributeBuilder>();
            Members = new List<EnumMemberBuilder>();
            Metadata = new List<MetadataBuilder>();

            Attributes.AddRange(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
            Members.AddRange(source.Members?.Select(x => new EnumMemberBuilder(x)) ?? Enumerable.Empty<EnumMemberBuilder>());
            Name = source.Name;
            Metadata.AddRange(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
            Visibility = source.Visibility;

            return this;
        }
        public EnumBuilder ClearAttributes()
        {
            Attributes.Clear();
            return this;
        }
        public EnumBuilder AddAttributes(IEnumerable<AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public EnumBuilder AddAttributes(params AttributeBuilder[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
            return this;
        }
        public EnumBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public EnumBuilder AddAttributes(params IAttribute[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            }
            return this;
        }
        public EnumBuilder ClearMembers()
        {
            Members.Clear();
            return this;
        }
        public EnumBuilder AddMembers(IEnumerable<EnumMemberBuilder> members)
        {
            return AddMembers(members.ToArray());
        }
        public EnumBuilder AddMembers(params EnumMemberBuilder[] members)
        {
            if (members != null)
            {
                Members.AddRange(members);
            }
            return this;
        }
        public EnumBuilder AddMembers(IEnumerable<IEnumMember> members)
        {
            return AddMembers(members.ToArray());
        }
        public EnumBuilder AddMembers(params IEnumMember[] members)
        {
            if (members != null)
            {
                Members.AddRange(members.Select(x => new EnumMemberBuilder(x)));
            }
            return this;
        }
        public EnumBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public EnumBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public EnumBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public EnumBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public EnumBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public EnumBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public EnumBuilder WithVisibility(Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }
        public EnumBuilder()
        {
            Attributes = new List<AttributeBuilder>();
            Members = new List<EnumMemberBuilder>();
            Metadata = new List<MetadataBuilder>();
        }
        public EnumBuilder(IEnum source)
        {
            Attributes = new List<AttributeBuilder>(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
            Members = new List<EnumMemberBuilder>(source.Members?.Select(x => new EnumMemberBuilder(x)) ?? Enumerable.Empty<EnumMemberBuilder>());
            Name = source.Name;
            Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
            Visibility = source.Visibility;
        }
    }
}
