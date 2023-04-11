﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.4
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
    public partial class SchemaBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Database.Builders.TableBuilder> Tables
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.StoredProcedureBuilder> StoredProcedures
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.ViewBuilder> Views
        {
            get;
            set;
        }

        public System.Text.StringBuilder Name
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

        public ModelFramework.Database.Contracts.ISchema Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.Schema(Tables.Select(x => x.Build()), StoredProcedures.Select(x => x.Build()), Views.Select(x => x.Build()), Name?.ToString(), Metadata.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public SchemaBuilder AddTables(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.TableBuilder> tables)
        {
            return AddTables(tables.ToArray());
        }

        public SchemaBuilder AddTables(params ModelFramework.Database.Builders.TableBuilder[] tables)
        {
            Tables.AddRange(tables);
            return this;
        }

        public SchemaBuilder AddStoredProcedures(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.StoredProcedureBuilder> storedProcedures)
        {
            return AddStoredProcedures(storedProcedures.ToArray());
        }

        public SchemaBuilder AddStoredProcedures(params ModelFramework.Database.Builders.StoredProcedureBuilder[] storedProcedures)
        {
            StoredProcedures.AddRange(storedProcedures);
            return this;
        }

        public SchemaBuilder AddViews(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.ViewBuilder> views)
        {
            return AddViews(views.ToArray());
        }

        public SchemaBuilder AddViews(params ModelFramework.Database.Builders.ViewBuilder[] views)
        {
            Views.AddRange(views);
            return this;
        }

        public SchemaBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public SchemaBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public SchemaBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public SchemaBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public SchemaBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public SchemaBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public SchemaBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public SchemaBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public SchemaBuilder()
        {
            Tables = new System.Collections.Generic.List<ModelFramework.Database.Builders.TableBuilder>();
            StoredProcedures = new System.Collections.Generic.List<ModelFramework.Database.Builders.StoredProcedureBuilder>();
            Views = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _nameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public SchemaBuilder(ModelFramework.Database.Contracts.ISchema source)
        {
            Tables = new System.Collections.Generic.List<ModelFramework.Database.Builders.TableBuilder>();
            StoredProcedures = new System.Collections.Generic.List<ModelFramework.Database.Builders.StoredProcedureBuilder>();
            Views = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Tables.AddRange(source.Tables.Select(x => new ModelFramework.Database.Builders.TableBuilder(x)));
            StoredProcedures.AddRange(source.StoredProcedures.Select(x => new ModelFramework.Database.Builders.StoredProcedureBuilder(x)));
            Views.AddRange(source.Views.Select(x => new ModelFramework.Database.Builders.ViewBuilder(x)));
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;
    }
#nullable restore
}

