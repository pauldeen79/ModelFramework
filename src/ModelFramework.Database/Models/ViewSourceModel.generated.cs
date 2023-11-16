﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.14
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
    public partial class ViewSourceModel
    {
        public string Alias
        {
            get;
            set;
        }

        public string SourceSchemaName
        {
            get;
            set;
        }

        public string SourceObjectName
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

        public ModelFramework.Database.Contracts.IViewSource ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.ViewSource(Alias, SourceSchemaName, SourceObjectName, Name, Metadata.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ViewSourceModel()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Alias = string.Empty;
            SourceSchemaName = string.Empty;
            SourceObjectName = string.Empty;
            Name = string.Empty;
        }

        public ViewSourceModel(ModelFramework.Database.Contracts.IViewSource source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Alias = source.Alias;
            SourceSchemaName = source.SourceSchemaName;
            SourceObjectName = source.SourceObjectName;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
        }
    }
#nullable restore
}

