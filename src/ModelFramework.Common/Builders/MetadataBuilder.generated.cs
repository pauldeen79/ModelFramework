﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.3
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Common.Builders
{
#nullable enable
    public partial class MetadataBuilder
    {
        public object? Value
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

        public ModelFramework.Common.Contracts.IMetadata Build()
        {
            return new ModelFramework.Common.Metadata(Value, Name);
        }

        public MetadataBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public MetadataBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public MetadataBuilder WithValue(System.Func<object?> valueDelegate)
        {
            _valueDelegate = new (valueDelegate);
            return this;
        }

        public MetadataBuilder WithValue(object? value)
        {
            Value = value;
            return this;
        }

        public MetadataBuilder()
        {
            _valueDelegate = new (() => default);
            _nameDelegate = new (() => string.Empty);
        }

        public MetadataBuilder(ModelFramework.Common.Contracts.IMetadata source)
        {
            _valueDelegate = new (() => source.Value);
            _nameDelegate = new (() => source.Name);
        }

        private System.Lazy<object?> _valueDelegate;

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

