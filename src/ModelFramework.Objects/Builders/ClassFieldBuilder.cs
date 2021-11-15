using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Builders
{
    public class ClassFieldBuilder
    {
        public bool Static { get; set; }
        public bool ReadOnly { get; set; }
        public bool Constant { get; set; }
        public object DefaultValue { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public Visibility Visibility { get; set; }
        public string Name { get; set; }
        public List<AttributeBuilder> Attributes { get; set; }
        public string TypeName { get; set; }
        public bool Virtual { get; set; }
        public bool Abstract { get; set; }
        public bool Protected { get; set; }
        public bool Override { get; set; }
        public bool Event { get; set; }
        public bool IsNullable { get; set; }
        public IClassField Build()
        {
            return new ClassField(Name,
                                  TypeName,
                                  Static,
                                  Constant,
                                  ReadOnly,
                                  Virtual,
                                  Abstract,
                                  Protected,
                                  Override,
                                  Event,
                                  IsNullable,
                                  DefaultValue,
                                  Visibility,
                                  Metadata.Select(x => x.Build()),
                                  Attributes.Select(x => x.Build()));
        }
        public ClassFieldBuilder Clear()
        {
            Static = default;
            ReadOnly = default;
            Constant = default;
            DefaultValue = default;
            Metadata.Clear();
            Visibility = default;
            Name = default;
            Attributes.Clear();
            TypeName = default;
            Virtual = default;
            Abstract = default;
            Protected = default;
            Override = default;
            Event = default;
            IsNullable = default;
            return this;
        }
        public ClassFieldBuilder Update(IClassField source)
        {
            Static = default;
            ReadOnly = default;
            Constant = default;
            DefaultValue = default;
            Metadata = new List<MetadataBuilder>();
            Visibility = default;
            Name = default;
            Attributes = new List<AttributeBuilder>();
            TypeName = default;
            Virtual = default;
            Abstract = default;
            Protected = default;
            Override = default;
            Event = default;
            IsNullable = default;
            if (source != null)
            {
                Static = source.Static;
                ReadOnly = source.ReadOnly;
                Constant = source.Constant;
                DefaultValue = source.DefaultValue;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                Visibility = source.Visibility;
                Name = source.Name;
                if (source.Attributes != null) Attributes.AddRange(source.Attributes.Select(x => new AttributeBuilder(x)));
                TypeName = source.TypeName;
                Virtual = source.Virtual;
                Abstract = source.Abstract;
                Protected = source.Protected;
                Override = source.Override;
                Event = source.Event;
                IsNullable = source.IsNullable;
            }
            return this;
        }
        public ClassFieldBuilder WithStatic(bool @static)
        {
            Static = @static;
            return this;
        }
        public ClassFieldBuilder WithReadOnly(bool readOnly)
        {
            ReadOnly = readOnly;
            return this;
        }
        public ClassFieldBuilder WithConstant(bool constant)
        {
            Constant = constant;
            return this;
        }
        public ClassFieldBuilder WithDefaultValue(object defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
        public ClassFieldBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ClassFieldBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassFieldBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public ClassFieldBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassFieldBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ClassFieldBuilder WithVisibility(Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }
        public ClassFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ClassFieldBuilder ClearAttributes()
        {
            Attributes.Clear();
            return this;
        }
        public ClassFieldBuilder AddAttributes(IEnumerable<AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassFieldBuilder AddAttributes(params AttributeBuilder[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
            return this;
        }
        public ClassFieldBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassFieldBuilder AddAttributes(params IAttribute[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            }
            return this;
        }
        public ClassFieldBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
            return this;
        }
        public ClassFieldBuilder WithVirtual(bool @virtual)
        {
            Virtual = @virtual;
            return this;
        }
        public ClassFieldBuilder WithAbstract(bool @abstract)
        {
            Abstract = @abstract;
            return this;
        }
        public ClassFieldBuilder WithProtected(bool @protected)
        {
            Protected = @protected;
            return this;
        }
        public ClassFieldBuilder WithOverride(bool @override)
        {
            Override = @override;
            return this;
        }
        public ClassFieldBuilder WithEvent(bool @event)
        {
            Event = @event;
            return this;
        }
        public ClassFieldBuilder WithIsNullable(bool isNullable)
        {
            IsNullable = isNullable;
            return this;
        }
        public ClassFieldBuilder(IClassField source = null)
        {
            if (source != null)
            {
                Static = source.Static;
                ReadOnly = source.ReadOnly;
                Constant = source.Constant;
                DefaultValue = source.DefaultValue;
                Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
                Visibility = source.Visibility;
                Name = source.Name;
                Attributes = new List<AttributeBuilder>(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
                TypeName = source.TypeName;
                Virtual = source.Virtual;
                Abstract = source.Abstract;
                Protected = source.Protected;
                Override = source.Override;
                Event = source.Event;
                IsNullable = source.IsNullable;
            }
            else
            {
                Metadata = new List<MetadataBuilder>();
                Attributes = new List<AttributeBuilder>();
            }
        }
#pragma warning disable S107 // Methods should not have too many parameters
        public ClassFieldBuilder(string name,
                                 string typeName,
                                 bool @static = false,
                                 bool constant = false,
                                 bool readOnly = false,
                                 bool @virtual = false,
                                 bool @abstract = false,
                                 bool @protected = false,
                                 bool @override = false,
                                 bool @event = false,
                                 bool isNullable = false,
                                 object defaultValue = null,
                                 Visibility visibility = Visibility.Private,
                                 IEnumerable<IMetadata> metadata = null,
                                 IEnumerable<IAttribute> attributes = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Metadata = new List<MetadataBuilder>();
            Attributes = new List<AttributeBuilder>();
            Name = name;
            TypeName = typeName;
            Static = @static;
            Constant = constant;
            ReadOnly = readOnly;
            Virtual = @virtual;
            Abstract = @abstract;
            Protected = @protected;
            Override = @override;
            Event = @event;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
            Visibility = visibility;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            if (attributes != null) Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
        }
    }
}
