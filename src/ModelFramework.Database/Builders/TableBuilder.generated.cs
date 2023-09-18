﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.11
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
    public partial class TableBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Database.Builders.PrimaryKeyConstraintBuilder> PrimaryKeyConstraints
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.UniqueConstraintBuilder> UniqueConstraints
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.DefaultValueConstraintBuilder> DefaultValueConstraints
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.ForeignKeyConstraintBuilder> ForeignKeyConstraints
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.IndexBuilder> Indexes
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.TableFieldBuilder> Fields
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public string FileGroupName
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.CheckConstraintBuilder> CheckConstraints
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.ITable Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.Table(PrimaryKeyConstraints.Select(x => x.Build()), UniqueConstraints.Select(x => x.Build()), DefaultValueConstraints.Select(x => x.Build()), ForeignKeyConstraints.Select(x => x.Build()), Indexes.Select(x => x.Build()), Fields.Select(x => x.Build()), Name, Metadata.Select(x => x.Build()), FileGroupName, CheckConstraints.Select(x => x.Build()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public TableBuilder AddPrimaryKeyConstraints(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.PrimaryKeyConstraintBuilder> primaryKeyConstraints)
        {
            return AddPrimaryKeyConstraints(primaryKeyConstraints.ToArray());
        }

        public TableBuilder AddPrimaryKeyConstraints(params ModelFramework.Database.Builders.PrimaryKeyConstraintBuilder[] primaryKeyConstraints)
        {
            PrimaryKeyConstraints.AddRange(primaryKeyConstraints);
            return this;
        }

        public TableBuilder AddUniqueConstraints(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.UniqueConstraintBuilder> uniqueConstraints)
        {
            return AddUniqueConstraints(uniqueConstraints.ToArray());
        }

        public TableBuilder AddUniqueConstraints(params ModelFramework.Database.Builders.UniqueConstraintBuilder[] uniqueConstraints)
        {
            UniqueConstraints.AddRange(uniqueConstraints);
            return this;
        }

        public TableBuilder AddDefaultValueConstraints(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.DefaultValueConstraintBuilder> defaultValueConstraints)
        {
            return AddDefaultValueConstraints(defaultValueConstraints.ToArray());
        }

        public TableBuilder AddDefaultValueConstraints(params ModelFramework.Database.Builders.DefaultValueConstraintBuilder[] defaultValueConstraints)
        {
            DefaultValueConstraints.AddRange(defaultValueConstraints);
            return this;
        }

        public TableBuilder AddForeignKeyConstraints(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.ForeignKeyConstraintBuilder> foreignKeyConstraints)
        {
            return AddForeignKeyConstraints(foreignKeyConstraints.ToArray());
        }

        public TableBuilder AddForeignKeyConstraints(params ModelFramework.Database.Builders.ForeignKeyConstraintBuilder[] foreignKeyConstraints)
        {
            ForeignKeyConstraints.AddRange(foreignKeyConstraints);
            return this;
        }

        public TableBuilder AddIndexes(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.IndexBuilder> indexes)
        {
            return AddIndexes(indexes.ToArray());
        }

        public TableBuilder AddIndexes(params ModelFramework.Database.Builders.IndexBuilder[] indexes)
        {
            Indexes.AddRange(indexes);
            return this;
        }

        public TableBuilder AddFields(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.TableFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }

        public TableBuilder AddFields(params ModelFramework.Database.Builders.TableFieldBuilder[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }

        public TableBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public TableBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public TableBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public TableBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public TableBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }

        public TableBuilder AddCheckConstraints(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.CheckConstraintBuilder> checkConstraints)
        {
            return AddCheckConstraints(checkConstraints.ToArray());
        }

        public TableBuilder AddCheckConstraints(params ModelFramework.Database.Builders.CheckConstraintBuilder[] checkConstraints)
        {
            CheckConstraints.AddRange(checkConstraints);
            return this;
        }

        public TableBuilder()
        {
            PrimaryKeyConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.PrimaryKeyConstraintBuilder>();
            UniqueConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.UniqueConstraintBuilder>();
            DefaultValueConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.DefaultValueConstraintBuilder>();
            ForeignKeyConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.ForeignKeyConstraintBuilder>();
            Indexes = new System.Collections.Generic.List<ModelFramework.Database.Builders.IndexBuilder>();
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Builders.TableFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            CheckConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.CheckConstraintBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            Name = string.Empty;
            FileGroupName = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public TableBuilder(ModelFramework.Database.Contracts.ITable source)
        {
            PrimaryKeyConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.PrimaryKeyConstraintBuilder>();
            UniqueConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.UniqueConstraintBuilder>();
            DefaultValueConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.DefaultValueConstraintBuilder>();
            ForeignKeyConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.ForeignKeyConstraintBuilder>();
            Indexes = new System.Collections.Generic.List<ModelFramework.Database.Builders.IndexBuilder>();
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Builders.TableFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            CheckConstraints = new System.Collections.Generic.List<ModelFramework.Database.Builders.CheckConstraintBuilder>();
            PrimaryKeyConstraints.AddRange(source.PrimaryKeyConstraints.Select(x => new ModelFramework.Database.Builders.PrimaryKeyConstraintBuilder(x)));
            UniqueConstraints.AddRange(source.UniqueConstraints.Select(x => new ModelFramework.Database.Builders.UniqueConstraintBuilder(x)));
            DefaultValueConstraints.AddRange(source.DefaultValueConstraints.Select(x => new ModelFramework.Database.Builders.DefaultValueConstraintBuilder(x)));
            ForeignKeyConstraints.AddRange(source.ForeignKeyConstraints.Select(x => new ModelFramework.Database.Builders.ForeignKeyConstraintBuilder(x)));
            Indexes.AddRange(source.Indexes.Select(x => new ModelFramework.Database.Builders.IndexBuilder(x)));
            Fields.AddRange(source.Fields.Select(x => new ModelFramework.Database.Builders.TableFieldBuilder(x)));
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            FileGroupName = source.FileGroupName;
            CheckConstraints.AddRange(source.CheckConstraints.Select(x => new ModelFramework.Database.Builders.CheckConstraintBuilder(x)));
        }
    }
#nullable restore
}

