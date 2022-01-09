﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 5.0.13
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
            return new ModelFramework.Common.Default.Metadata(Name, Value);
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