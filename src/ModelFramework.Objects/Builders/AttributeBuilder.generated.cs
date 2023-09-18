﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.11
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

        public string Name
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.IAttribute Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.Attribute(Parameters.Select(x => x.Build()), Metadata.Select(x => x.Build()), Name);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
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

        public AttributeBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public AttributeBuilder()
        {
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeParameterBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            Name = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public AttributeBuilder(ModelFramework.Objects.Contracts.IAttribute source)
        {
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeParameterBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Objects.Builders.AttributeParameterBuilder(x)));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            Name = source.Name;
        }
    }
#nullable restore
}

