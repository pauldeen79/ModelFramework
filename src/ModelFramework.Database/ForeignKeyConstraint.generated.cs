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

namespace ModelFramework.Database
{
#nullable enable
    public partial record ForeignKeyConstraint : ModelFramework.Database.Contracts.IForeignKeyConstraint
    {
        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IForeignKeyConstraintField> LocalFields
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IForeignKeyConstraintField> ForeignFields
        {
            get;
        }

        public string ForeignTableName
        {
            get;
        }

        public ModelFramework.Database.Contracts.CascadeAction CascadeUpdate
        {
            get;
        }

        public ModelFramework.Database.Contracts.CascadeAction CascadeDelete
        {
            get;
        }

        public string Name
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
        {
            get;
        }

        public ForeignKeyConstraint(System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IForeignKeyConstraintField> localFields, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IForeignKeyConstraintField> foreignFields, string foreignTableName, ModelFramework.Database.Contracts.CascadeAction cascadeUpdate, ModelFramework.Database.Contracts.CascadeAction cascadeDelete, string name, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata)
        {
            this.LocalFields = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IForeignKeyConstraintField>(localFields);
            this.ForeignFields = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IForeignKeyConstraintField>(foreignFields);
            this.ForeignTableName = foreignTableName;
            this.CascadeUpdate = cascadeUpdate;
            this.CascadeDelete = cascadeDelete;
            this.Name = name;
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

