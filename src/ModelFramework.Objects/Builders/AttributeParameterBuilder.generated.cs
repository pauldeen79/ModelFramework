﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.5
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
    public partial class AttributeParameterBuilder
    {
        public object Value
        {
            get
            {
                return _valueDelegate.Value;
            }
            set
            {
                _valueDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public string Name
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

        public AttributeParameterBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public AttributeParameterBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public AttributeParameterBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Objects.Contracts.IAttributeParameter Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.AttributeParameter(Value, Metadata.Select(x => x.Build()), Name);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public AttributeParameterBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public AttributeParameterBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public AttributeParameterBuilder WithValue(System.Func<object> valueDelegate)
        {
            _valueDelegate = new (valueDelegate);
            return this;
        }

        public AttributeParameterBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }

        public AttributeParameterBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _valueDelegate = new (() => new object());
            _nameDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public AttributeParameterBuilder(ModelFramework.Objects.Contracts.IAttributeParameter source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _valueDelegate = new (() => source.Value);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _nameDelegate = new (() => source.Name);
        }

        private System.Lazy<object> _valueDelegate;

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

