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

namespace ModelFramework.Database.Models
{
#nullable enable
    public partial class DefaultValueConstraintModel
    {
        public string FieldName
        {
            get;
            set;
        }

        public string DefaultValue
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.IDefaultValueConstraint ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.DefaultValueConstraint(FieldName, DefaultValue, Name, Metadata.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public DefaultValueConstraintModel()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            FieldName = string.Empty;
            DefaultValue = string.Empty;
            Name = string.Empty;
        }

        public DefaultValueConstraintModel(ModelFramework.Database.Contracts.IDefaultValueConstraint source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            FieldName = source.FieldName;
            DefaultValue = source.DefaultValue;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
        }
    }
#nullable restore
}

