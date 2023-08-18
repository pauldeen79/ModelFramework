﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.10
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
    public partial class IndexFieldModel
    {
        public bool IsDescending
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

        public ModelFramework.Database.Contracts.IIndexField ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.IndexField(IsDescending, Name, Metadata.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public IndexFieldModel()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            #pragma warning disable CS8603 // Possible null reference return.
            IsDescending = default(System.Boolean)!;
            Name = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public IndexFieldModel(ModelFramework.Database.Contracts.IIndexField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            IsDescending = source.IsDescending;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
        }
    }
#nullable restore
}
