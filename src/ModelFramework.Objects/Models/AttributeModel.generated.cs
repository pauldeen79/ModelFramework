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

namespace ModelFramework.Objects.Models
{
#nullable enable
    public partial class AttributeModel
    {
        public System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeParameterModel> Parameters
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

        public ModelFramework.Objects.Contracts.IAttribute ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.Attribute(Parameters.Select(x => x.ToEntity()), Metadata.Select(x => x.ToEntity()), Name);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public AttributeModel()
        {
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeParameterModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Name = string.Empty;
        }

        public AttributeModel(ModelFramework.Objects.Contracts.IAttribute source)
        {
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeParameterModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Objects.Models.AttributeParameterModel(x)));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
            Name = source.Name;
        }
    }
#nullable restore
}

