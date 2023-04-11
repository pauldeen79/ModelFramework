﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.4
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

        public ModelFramework.Common.Contracts.IMetadata Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Common.Metadata(Value, Name?.ToString());
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public MetadataBuilder WithValue(object? value)
        {
            Value = value;
            return this;
        }

        public MetadataBuilder WithValue(System.Func<object?> valueDelegate)
        {
            _valueDelegate = new (valueDelegate);
            return this;
        }

        public MetadataBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public MetadataBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public MetadataBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public MetadataBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public MetadataBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public MetadataBuilder()
        {
            #pragma warning disable CS8603 // Possible null reference return.
            _valueDelegate = new (() => default);
            _nameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public MetadataBuilder(ModelFramework.Common.Contracts.IMetadata source)
        {
            _valueDelegate = new (() => source.Value);
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
        }

        protected System.Lazy<object?> _valueDelegate;

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;
    }
#nullable restore
}

