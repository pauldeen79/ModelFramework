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

namespace ModelFramework.Objects.Models
{
#nullable enable
    public partial class EnumMemberModel
    {
        public object? Value
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel> Attributes
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

        public ModelFramework.Objects.Contracts.IEnumMember ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.EnumMember(Value, Attributes.Select(x => x.ToEntity()), Name, Metadata.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public EnumMemberModel()
        {
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Name = string.Empty;
        }

        public EnumMemberModel(ModelFramework.Objects.Contracts.IEnumMember source)
        {
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Value = source.Value;
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Models.AttributeModel(x)));
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
        }
    }
#nullable restore
}

