using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class TableBuilder
    {
        public string FileGroupName { get; set; }
        public List<PrimaryKeyConstraintBuilder> PrimaryKeyConstraints { get; set; }
        public List<UniqueConstraintBuilder> UniqueConstraints { get; set; }
        public List<IndexBuilder> Indexes { get; set; }
        public List<TableFieldBuilder> Fields { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public List<DefaultValueConstraintBuilder> DefaultValueConstraints { get; set; }
        public List<ForeignKeyConstraintBuilder> ForeignKeyConstraints { get; set; }
        public List<CheckConstraintBuilder> CheckConstraints { get; set; }
        public ITable Build()
        {
            return new Table(Name,
                             FileGroupName,
                             Fields.Select(x => x.Build()),
                             PrimaryKeyConstraints.Select(x => x.Build()),
                             UniqueConstraints.Select(x => x.Build()),
                             DefaultValueConstraints.Select(x => x.Build()),
                             ForeignKeyConstraints.Select(x => x.Build()),
                             Indexes.Select(x => x.Build()),
                             CheckConstraints.Select(x => x.Build()),
                             Metadata.Select(x => x.Build()));
        }
        public TableBuilder Clear()
        {
            FileGroupName = string.Empty;
            PrimaryKeyConstraints.Clear();
            UniqueConstraints.Clear();
            Indexes.Clear();
            Fields.Clear();
            Name = string.Empty;
            Metadata.Clear();
            DefaultValueConstraints.Clear();
            ForeignKeyConstraints.Clear();
            CheckConstraints.Clear();
            return this;
        }
        public TableBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }
        public TableBuilder ClearPrimaryKeyConstraints()
        {
            PrimaryKeyConstraints.Clear();
            return this;
        }
        public TableBuilder AddPrimaryKeyConstraints(IEnumerable<PrimaryKeyConstraintBuilder> primaryKeyConstraints)
        {
            return AddPrimaryKeyConstraints(primaryKeyConstraints.ToArray());
        }
        public TableBuilder AddPrimaryKeyConstraints(params PrimaryKeyConstraintBuilder[] primaryKeyConstraints)
        {
            PrimaryKeyConstraints.AddRange(primaryKeyConstraints);
            return this;
        }
        public TableBuilder AddPrimaryKeyConstraints(IEnumerable<IPrimaryKeyConstraint> primaryKeyConstraints)
        {
            return AddPrimaryKeyConstraints(primaryKeyConstraints.ToArray());
        }
        public TableBuilder AddPrimaryKeyConstraints(params IPrimaryKeyConstraint[] primaryKeyConstraints)
        {
            PrimaryKeyConstraints.AddRange(primaryKeyConstraints.Select(itemToAdd => new PrimaryKeyConstraintBuilder(itemToAdd)));
            return this;
        }
        public TableBuilder ClearUniqueConstraints()
        {
            UniqueConstraints.Clear();
            return this;
        }
        public TableBuilder AddUniqueConstraints(IEnumerable<UniqueConstraintBuilder> uniqueConstraints)
        {
            return AddUniqueConstraints(uniqueConstraints.ToArray());
        }
        public TableBuilder AddUniqueConstraints(params UniqueConstraintBuilder[] uniqueConstraints)
        {
            UniqueConstraints.AddRange(uniqueConstraints);
            return this;
        }
        public TableBuilder AddUniqueConstraints(IEnumerable<IUniqueConstraint> uniqueConstraints)
        {
            return AddUniqueConstraints(uniqueConstraints.ToArray());
        }
        public TableBuilder AddUniqueConstraints(params IUniqueConstraint[] uniqueConstraints)
        {
            UniqueConstraints.AddRange(uniqueConstraints.Select(itemToAdd => new UniqueConstraintBuilder(itemToAdd)));
            return this;
        }
        public TableBuilder ClearIndexes()
        {
            Indexes.Clear();
            return this;
        }
        public TableBuilder AddIndexes(IEnumerable<IndexBuilder> indexes)
        {
            return AddIndexes(indexes.ToArray());
        }
        public TableBuilder AddIndexes(params IndexBuilder[] indexes)
        {
            Indexes.AddRange(indexes);
            return this;
        }
        public TableBuilder AddIndexes(IEnumerable<IIndex> indexes)
        {
            return AddIndexes(indexes.ToArray());
        }
        public TableBuilder AddIndexes(params IIndex[] indexes)
        {
            Indexes.AddRange(indexes.Select(itemToAdd => new IndexBuilder(itemToAdd)));
            return this;
        }
        public TableBuilder ClearFields()
        {
            Fields.Clear();
            return this;
        }
        public TableBuilder AddFields(IEnumerable<TableFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }
        public TableBuilder AddFields(params TableFieldBuilder[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }
        public TableBuilder AddFields(IEnumerable<ITableField> fields)
        {
            return AddFields(fields.ToArray());
        }
        public TableBuilder AddFields(params ITableField[] fields)
        {
            Fields.AddRange(fields.Select(itemToAdd => new TableFieldBuilder(itemToAdd)));
            return this;
        }
        public TableBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public TableBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public TableBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public TableBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public TableBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public TableBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public TableBuilder ClearDefaultValueConstraints()
        {
            DefaultValueConstraints.Clear();
            return this;
        }
        public TableBuilder AddDefaultValueConstraints(IEnumerable<DefaultValueConstraintBuilder> defaultValueConstraints)
        {
            return AddDefaultValueConstraints(defaultValueConstraints.ToArray());
        }
        public TableBuilder AddDefaultValueConstraints(params DefaultValueConstraintBuilder[] defaultValueConstraints)
        {
            DefaultValueConstraints.AddRange(defaultValueConstraints);
            return this;
        }
        public TableBuilder AddDefaultValueConstraints(IEnumerable<IDefaultValueConstraint> defaultValueConstraints)
        {
            return AddDefaultValueConstraints(defaultValueConstraints.ToArray());
        }
        public TableBuilder AddDefaultValueConstraints(params IDefaultValueConstraint[] defaultValueConstraints)
        {
            DefaultValueConstraints.AddRange(defaultValueConstraints.Select(itemToAdd => new DefaultValueConstraintBuilder(itemToAdd)));
            return this;
        }
        public TableBuilder ClearForeignKeyConstraints()
        {
            ForeignKeyConstraints.Clear();
            return this;
        }
        public TableBuilder AddForeignKeyConstraints(IEnumerable<ForeignKeyConstraintBuilder> foreignKeyConstraints)
        {
            return AddForeignKeyConstraints(foreignKeyConstraints.ToArray());
        }
        public TableBuilder AddForeignKeyConstraints(params ForeignKeyConstraintBuilder[] foreignKeyConstraints)
        {
            ForeignKeyConstraints.AddRange(foreignKeyConstraints);
            return this;
        }
        public TableBuilder AddForeignKeyConstraints(IEnumerable<IForeignKeyConstraint> foreignKeyConstraints)
        {
            return AddForeignKeyConstraints(foreignKeyConstraints.ToArray());
        }
        public TableBuilder AddForeignKeyConstraints(params IForeignKeyConstraint[] foreignKeyConstraints)
        {
            ForeignKeyConstraints.AddRange(foreignKeyConstraints.Select(itemToAdd => new ForeignKeyConstraintBuilder(itemToAdd)));
            return this;
        }
        public TableBuilder ClearCheckConstraints()
        {
            CheckConstraints.Clear();
            return this;
        }
        public TableBuilder AddCheckConstraints(IEnumerable<CheckConstraintBuilder> checkConstraints)
        {
            return AddCheckConstraints(checkConstraints.ToArray());
        }
        public TableBuilder AddCheckConstraints(params CheckConstraintBuilder[] checkConstraints)
        {
            CheckConstraints.AddRange(checkConstraints);
            return this;
        }
        public TableBuilder AddCheckConstraints(IEnumerable<ICheckConstraint> checkConstraints)
        {
            return AddCheckConstraints(checkConstraints.ToArray());
        }
        public TableBuilder AddCheckConstraints(params ICheckConstraint[] checkConstraints)
        {
            CheckConstraints.AddRange(checkConstraints.Select(itemToAdd => new CheckConstraintBuilder(itemToAdd)));
            return this;
        }
        public TableBuilder()
        {
            Name = string.Empty;
            FileGroupName = string.Empty;
            PrimaryKeyConstraints = new List<PrimaryKeyConstraintBuilder>();
            UniqueConstraints = new List<UniqueConstraintBuilder>();
            Indexes = new List<IndexBuilder>();
            Fields = new List<TableFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
            DefaultValueConstraints = new List<DefaultValueConstraintBuilder>();
            ForeignKeyConstraints = new List<ForeignKeyConstraintBuilder>();
            CheckConstraints = new List<CheckConstraintBuilder>();
        }
        public TableBuilder(ITable source)
        {
            PrimaryKeyConstraints = new List<PrimaryKeyConstraintBuilder>();
            UniqueConstraints = new List<UniqueConstraintBuilder>();
            Indexes = new List<IndexBuilder>();
            Fields = new List<TableFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
            DefaultValueConstraints = new List<DefaultValueConstraintBuilder>();
            ForeignKeyConstraints = new List<ForeignKeyConstraintBuilder>();
            CheckConstraints = new List<CheckConstraintBuilder>();

            FileGroupName = source.FileGroupName;
            PrimaryKeyConstraints.AddRange(source.PrimaryKeyConstraints.Select(x => new PrimaryKeyConstraintBuilder(x)));
            UniqueConstraints.AddRange(source.UniqueConstraints.Select(x => new UniqueConstraintBuilder(x)));
            Indexes.AddRange(source.Indexes.Select(x => new IndexBuilder(x)));
            Fields.AddRange(source.Fields.Select(x => new TableFieldBuilder(x)));
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
            DefaultValueConstraints.AddRange(source.DefaultValueConstraints.Select(x => new DefaultValueConstraintBuilder(x)));
            ForeignKeyConstraints.AddRange(source.ForeignKeyConstraints.Select(x => new ForeignKeyConstraintBuilder(x)));
            CheckConstraints.AddRange(source.CheckConstraints.Select(x => new CheckConstraintBuilder(x)));
        }
    }
}
