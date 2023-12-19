﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 8.0.0
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
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ModelFramework.Common.Contracts.IMetadata Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Common.Metadata(Value, Name);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public MetadataBuilder WithValue(object? value)
        {
            Value = value;
            return this;
        }

        public MetadataBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public MetadataBuilder()
        {
            Name = string.Empty;
        }

        public MetadataBuilder(ModelFramework.Common.Contracts.IMetadata source)
        {
            Value = source.Value;
            Name = source.Name;
        }
    }
#nullable restore
}

