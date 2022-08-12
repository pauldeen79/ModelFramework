﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.8
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
    public partial record Table : ModelFramework.Database.Contracts.ITable
    {
        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IPrimaryKeyConstraint> PrimaryKeyConstraints
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IUniqueConstraint> UniqueConstraints
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IDefaultValueConstraint> DefaultValueConstraints
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IForeignKeyConstraint> ForeignKeyConstraints
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IIndex> Indexes
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.ITableField> Fields
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

        public string FileGroupName
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.ICheckConstraint> CheckConstraints
        {
            get;
        }

        public Table(System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IPrimaryKeyConstraint> primaryKeyConstraints, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IUniqueConstraint> uniqueConstraints, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IDefaultValueConstraint> defaultValueConstraints, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IForeignKeyConstraint> foreignKeyConstraints, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IIndex> indexes, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.ITableField> fields, string name, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, string fileGroupName, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.ICheckConstraint> checkConstraints)
        {
            this.PrimaryKeyConstraints = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IPrimaryKeyConstraint>(primaryKeyConstraints);
            this.UniqueConstraints = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IUniqueConstraint>(uniqueConstraints);
            this.DefaultValueConstraints = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IDefaultValueConstraint>(defaultValueConstraints);
            this.ForeignKeyConstraints = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IForeignKeyConstraint>(foreignKeyConstraints);
            this.Indexes = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IIndex>(indexes);
            this.Fields = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.ITableField>(fields);
            this.Name = name;
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.FileGroupName = fileGroupName;
            this.CheckConstraints = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.ICheckConstraint>(checkConstraints);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

