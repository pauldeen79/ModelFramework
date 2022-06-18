﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.6
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Database.Builders
{
#nullable enable
    public partial class ForeignKeyConstraintBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder> LocalFields
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder> ForeignFields
        {
            get;
            set;
        }

        public string ForeignTableName
        {
            get
            {
                return _foreignTableNameDelegate.Value;
            }
            set
            {
                _foreignTableNameDelegate = new (() => value);
            }
        }

        public ModelFramework.Database.Contracts.CascadeAction CascadeUpdate
        {
            get
            {
                return _cascadeUpdateDelegate.Value;
            }
            set
            {
                _cascadeUpdateDelegate = new (() => value);
            }
        }

        public ModelFramework.Database.Contracts.CascadeAction CascadeDelete
        {
            get
            {
                return _cascadeDeleteDelegate.Value;
            }
            set
            {
                _cascadeDeleteDelegate = new (() => value);
            }
        }

        public string Name
        {
            get
            {
                return _nameDelegate.Value;
            }
            set
            {
                _nameDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public ForeignKeyConstraintBuilder AddForeignFields(params ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder[] foreignFields)
        {
            ForeignFields.AddRange(foreignFields);
            return this;
        }

        public ForeignKeyConstraintBuilder AddForeignFields(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder> foreignFields)
        {
            return AddForeignFields(foreignFields.ToArray());
        }

        public ForeignKeyConstraintBuilder AddLocalFields(params ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder[] localFields)
        {
            LocalFields.AddRange(localFields);
            return this;
        }

        public ForeignKeyConstraintBuilder AddLocalFields(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder> localFields)
        {
            return AddLocalFields(localFields.ToArray());
        }

        public ForeignKeyConstraintBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ForeignKeyConstraintBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ForeignKeyConstraintBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.IForeignKeyConstraint Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.ForeignKeyConstraint(LocalFields.Select(x => x.Build()), ForeignFields.Select(x => x.Build()), ForeignTableName, CascadeUpdate, CascadeDelete, Name, Metadata.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ForeignKeyConstraintBuilder WithCascadeDelete(ModelFramework.Database.Contracts.CascadeAction cascadeDelete)
        {
            CascadeDelete = cascadeDelete;
            return this;
        }

        public ForeignKeyConstraintBuilder WithCascadeDelete(System.Func<ModelFramework.Database.Contracts.CascadeAction> cascadeDeleteDelegate)
        {
            _cascadeDeleteDelegate = new (cascadeDeleteDelegate);
            return this;
        }

        public ForeignKeyConstraintBuilder WithCascadeUpdate(ModelFramework.Database.Contracts.CascadeAction cascadeUpdate)
        {
            CascadeUpdate = cascadeUpdate;
            return this;
        }

        public ForeignKeyConstraintBuilder WithCascadeUpdate(System.Func<ModelFramework.Database.Contracts.CascadeAction> cascadeUpdateDelegate)
        {
            _cascadeUpdateDelegate = new (cascadeUpdateDelegate);
            return this;
        }

        public ForeignKeyConstraintBuilder WithForeignTableName(System.Func<string> foreignTableNameDelegate)
        {
            _foreignTableNameDelegate = new (foreignTableNameDelegate);
            return this;
        }

        public ForeignKeyConstraintBuilder WithForeignTableName(string foreignTableName)
        {
            ForeignTableName = foreignTableName;
            return this;
        }

        public ForeignKeyConstraintBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ForeignKeyConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ForeignKeyConstraintBuilder()
        {
            LocalFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder>();
            ForeignFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _foreignTableNameDelegate = new (() => string.Empty);
            _cascadeUpdateDelegate = new (() => default);
            _cascadeDeleteDelegate = new (() => default);
            _nameDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ForeignKeyConstraintBuilder(ModelFramework.Database.Contracts.IForeignKeyConstraint source)
        {
            LocalFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder>();
            ForeignFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            LocalFields.AddRange(source.LocalFields.Select(x => new ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder(x)));
            ForeignFields.AddRange(source.ForeignFields.Select(x => new ModelFramework.Database.Builders.ForeignKeyConstraintFieldBuilder(x)));
            _foreignTableNameDelegate = new (() => source.ForeignTableName);
            _cascadeUpdateDelegate = new (() => source.CascadeUpdate);
            _cascadeDeleteDelegate = new (() => source.CascadeDelete);
            _nameDelegate = new (() => source.Name);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        private System.Lazy<string> _foreignTableNameDelegate;

        private System.Lazy<ModelFramework.Database.Contracts.CascadeAction> _cascadeUpdateDelegate;

        private System.Lazy<ModelFramework.Database.Contracts.CascadeAction> _cascadeDeleteDelegate;

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

