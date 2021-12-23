﻿using System;
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
        public object? DefaultValue { get; set; }
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
            TypeName = string.Empty;
            Attributes.Clear();
            Metadata.Clear();
            Name = string.Empty;
            DefaultValue = default;
            IsNullable = default;
            return this;
        }
        public ParameterBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
            return this;
        }
        public ParameterBuilder WithType(Type type)
        {
            TypeName = type.FullName;
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
            Attributes.AddRange(attributes);
            return this;
        }
        public ParameterBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ParameterBuilder AddAttributes(params IAttribute[] attributes)
        {
            Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
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
            Metadata.AddRange(metadata);
            return this;
        }
        public ParameterBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ParameterBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public ParameterBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ParameterBuilder WithDefaultValue(object? defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
        public ParameterBuilder WithIsNullable(bool isNullable = true)
        {
            IsNullable = isNullable;
            return this;
        }
        public ParameterBuilder()
        {
            Attributes = new List<AttributeBuilder>();
            Metadata = new List<MetadataBuilder>();
            Name = string.Empty;
            TypeName = string.Empty;
        }
        public ParameterBuilder(IParameter source)
        {
            TypeName = source.TypeName;
            Attributes = new List<AttributeBuilder>(source.Attributes.Select(x => new AttributeBuilder(x)));
            Metadata = new List<MetadataBuilder>(source.Metadata.Select(x => new MetadataBuilder(x)));
            Name = source.Name;
            DefaultValue = source.DefaultValue;
            IsNullable = source.IsNullable;
        }
    }
}
