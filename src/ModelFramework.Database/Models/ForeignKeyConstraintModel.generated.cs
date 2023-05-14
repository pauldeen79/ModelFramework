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
    public partial class ForeignKeyConstraintModel
    {
        public System.Collections.Generic.List<ModelFramework.Database.Models.ForeignKeyConstraintFieldModel> LocalFields
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Models.ForeignKeyConstraintFieldModel> ForeignFields
        {
            get;
            set;
        }

        public string ForeignTableName
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.CascadeAction CascadeUpdate
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.CascadeAction CascadeDelete
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

        public ModelFramework.Database.Contracts.IForeignKeyConstraint ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.ForeignKeyConstraint(LocalFields.Select(x => x.ToEntity()), ForeignFields.Select(x => x.ToEntity()), ForeignTableName, CascadeUpdate, CascadeDelete, Name, Metadata.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ForeignKeyConstraintModel()
        {
            LocalFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ForeignKeyConstraintFieldModel>();
            ForeignFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ForeignKeyConstraintFieldModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            #pragma warning disable CS8603 // Possible null reference return.
            ForeignTableName = string.Empty;
            CascadeUpdate = default(ModelFramework.Database.Contracts.CascadeAction)!;
            CascadeDelete = default(ModelFramework.Database.Contracts.CascadeAction)!;
            Name = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ForeignKeyConstraintModel(ModelFramework.Database.Contracts.IForeignKeyConstraint source)
        {
            LocalFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ForeignKeyConstraintFieldModel>();
            ForeignFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ForeignKeyConstraintFieldModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            LocalFields.AddRange(source.LocalFields.Select(x => new ModelFramework.Database.Models.ForeignKeyConstraintFieldModel(x)));
            ForeignFields.AddRange(source.ForeignFields.Select(x => new ModelFramework.Database.Models.ForeignKeyConstraintFieldModel(x)));
            ForeignTableName = source.ForeignTableName;
            CascadeUpdate = source.CascadeUpdate;
            CascadeDelete = source.CascadeDelete;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
        }
    }
#nullable restore
}

