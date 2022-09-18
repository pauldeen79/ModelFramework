﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.9
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Objects.Builders
{
#nullable enable
    public partial class AttributeBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeParameterBuilder> Parameters
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public System.Text.StringBuilder Name
        {
            get
            {
                return _nameDelegate.Value;
            }
            set
            {
                _nameDelegate = new (() => value);
            }
        }

        public ModelFramework.Objects.Contracts.IAttribute Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.Attribute(Parameters.Select(x => x.Build()), Metadata.Select(x => x.Build()), Name?.ToString());
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public AttributeBuilder AddParameters(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }

        public AttributeBuilder AddParameters(params ModelFramework.Objects.Builders.AttributeParameterBuilder[] parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }

        public AttributeBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public AttributeBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public AttributeBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public AttributeBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public AttributeBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public AttributeBuilder WithName(string value)
        {
            Name.Clear().Append(value);
            return this;
        }

        public AttributeBuilder AppendToName(string value)
        {
            Name.Append(value);
            return this;
        }

        public AttributeBuilder AppendLineToName(string value)
        {
            Name.AppendLine(value);
            return this;
        }

        public AttributeBuilder()
        {
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeParameterBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _nameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public AttributeBuilder(ModelFramework.Objects.Contracts.IAttribute source)
        {
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeParameterBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Objects.Builders.AttributeParameterBuilder(x)));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
        }

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;
    }
#nullable restore
}

