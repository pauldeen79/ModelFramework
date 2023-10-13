﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.12
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
    public partial class EnumModel
    {
        public System.Collections.Generic.List<ModelFramework.Objects.Models.EnumMemberModel> Members
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel> Attributes
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel> Metadata
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.IEnum ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.Enum(Members.Select(x => x.ToEntity()), Attributes.Select(x => x.ToEntity()), Metadata.Select(x => x.ToEntity()), Name, Visibility);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public EnumModel()
        {
            Members = new System.Collections.Generic.List<ModelFramework.Objects.Models.EnumMemberModel>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Name = string.Empty;
        }

        public EnumModel(ModelFramework.Objects.Contracts.IEnum source)
        {
            Members = new System.Collections.Generic.List<ModelFramework.Objects.Models.EnumMemberModel>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Members.AddRange(source.Members.Select(x => new ModelFramework.Objects.Models.EnumMemberModel(x)));
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Models.AttributeModel(x)));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
            Name = source.Name;
            Visibility = source.Visibility;
        }
    }
#nullable restore
}

