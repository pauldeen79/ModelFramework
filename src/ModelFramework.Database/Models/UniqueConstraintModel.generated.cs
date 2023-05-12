﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.5
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
    public partial class UniqueConstraintModel
    {
        public System.Collections.Generic.List<ModelFramework.Database.Models.UniqueConstraintFieldModel> Fields
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

        public string FileGroupName
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.IUniqueConstraint ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.UniqueConstraint(Fields.Select(x => x.ToEntity()), Name, Metadata.Select(x => x.ToEntity()), FileGroupName);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public UniqueConstraintModel()
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Models.UniqueConstraintFieldModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            #pragma warning disable CS8603 // Possible null reference return.
            Name = string.Empty;
            FileGroupName = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public UniqueConstraintModel(ModelFramework.Database.Contracts.IUniqueConstraint source)
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Models.UniqueConstraintFieldModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Fields.AddRange(source.Fields.Select(x => new ModelFramework.Database.Models.UniqueConstraintFieldModel(x)));
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
            FileGroupName = source.FileGroupName;
        }
    }
#nullable restore
}

