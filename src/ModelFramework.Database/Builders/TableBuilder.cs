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
            FileGroupName = default;
            PrimaryKeyConstraints.Clear();
            UniqueConstraints.Clear();
            Indexes.Clear();
            Fields.Clear();
            Name = default;
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
            if (primaryKeyConstraints != null)
            {
                foreach (var itemToAdd in primaryKeyConstraints)
                {
                    PrimaryKeyConstraints.Add(itemToAdd);
                }
            }
            return this;
        }
        public TableBuilder AddPrimaryKeyConstraints(IEnumerable<IPrimaryKeyConstraint> primaryKeyConstraints)
        {
            return AddPrimaryKeyConstraints(primaryKeyConstraints.ToArray());
        }
        public TableBuilder AddPrimaryKeyConstraints(params IPrimaryKeyConstraint[] primaryKeyConstraints)
        {
            if (primaryKeyConstraints != null)
            {
                foreach (var itemToAdd in primaryKeyConstraints)
                {
                    PrimaryKeyConstraints.Add(new PrimaryKeyConstraintBuilder(itemToAdd));
                }
            }
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
            if (uniqueConstraints != null)
            {
                foreach (var itemToAdd in uniqueConstraints)
                {
                    UniqueConstraints.Add(itemToAdd);
                }
            }
            return this;
        }
        public TableBuilder AddUniqueConstraints(IEnumerable<IUniqueConstraint> uniqueConstraints)
        {
            return AddUniqueConstraints(uniqueConstraints.ToArray());
        }
        public TableBuilder AddUniqueConstraints(params IUniqueConstraint[] uniqueConstraints)
        {
            if (uniqueConstraints != null)
            {
                foreach (var itemToAdd in uniqueConstraints)
                {
                    UniqueConstraints.Add(new UniqueConstraintBuilder(itemToAdd));
                }
            }
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
            if (indexes != null)
            {
                foreach (var itemToAdd in indexes)
                {
                    Indexes.Add(itemToAdd);
                }
            }
            return this;
        }
        public TableBuilder AddIndexes(IEnumerable<IIndex> indexes)
        {
            return AddIndexes(indexes.ToArray());
        }
        public TableBuilder AddIndexes(params IIndex[] indexes)
        {
            if (indexes != null)
            {
                foreach (var itemToAdd in indexes)
                {
                    Indexes.Add(new IndexBuilder(itemToAdd));
                }
            }
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
            if (fields != null)
            {
                foreach (var itemToAdd in fields)
                {
                    Fields.Add(itemToAdd);
                }
            }
            return this;
        }
        public TableBuilder AddFields(IEnumerable<ITableField> fields)
        {
            return AddFields(fields.ToArray());
        }
        public TableBuilder AddFields(params ITableField[] fields)
        {
            if (fields != null)
            {
                foreach (var itemToAdd in fields)
                {
                    Fields.Add(new TableFieldBuilder(itemToAdd));
                }
            }
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
            if (metadata != null)
            {
                foreach (var itemToAdd in metadata)
                {
                    Metadata.Add(itemToAdd);
                }
            }
            return this;
        }
        public TableBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public TableBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
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
            if (defaultValueConstraints != null)
            {
                foreach (var itemToAdd in defaultValueConstraints)
                {
                    DefaultValueConstraints.Add(itemToAdd);
                }
            }
            return this;
        }
        public TableBuilder AddDefaultValueConstraints(IEnumerable<IDefaultValueConstraint> defaultValueConstraints)
        {
            return AddDefaultValueConstraints(defaultValueConstraints.ToArray());
        }
        public TableBuilder AddDefaultValueConstraints(params IDefaultValueConstraint[] defaultValueConstraints)
        {
            if (defaultValueConstraints != null)
            {
                foreach (var itemToAdd in defaultValueConstraints)
                {
                    DefaultValueConstraints.Add(new DefaultValueConstraintBuilder(itemToAdd));
                }
            }
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
            if (foreignKeyConstraints != null)
            {
                foreach (var itemToAdd in foreignKeyConstraints)
                {
                    ForeignKeyConstraints.Add(itemToAdd);
                }
            }
            return this;
        }
        public TableBuilder AddForeignKeyConstraints(IEnumerable<IForeignKeyConstraint> foreignKeyConstraints)
        {
            return AddForeignKeyConstraints(foreignKeyConstraints.ToArray());
        }
        public TableBuilder AddForeignKeyConstraints(params IForeignKeyConstraint[] foreignKeyConstraints)
        {
            if (foreignKeyConstraints != null)
            {
                foreach (var itemToAdd in foreignKeyConstraints)
                {
                    ForeignKeyConstraints.Add(new ForeignKeyConstraintBuilder(itemToAdd));
                }
            }
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
            if (checkConstraints != null)
            {
                foreach (var itemToAdd in checkConstraints)
                {
                    CheckConstraints.Add(itemToAdd);
                }
            }
            return this;
        }
        public TableBuilder AddCheckConstraints(IEnumerable<ICheckConstraint> checkConstraints)
        {
            return AddCheckConstraints(checkConstraints.ToArray());
        }
        public TableBuilder AddCheckConstraints(params ICheckConstraint[] checkConstraints)
        {
            if (checkConstraints != null)
            {
                foreach (var itemToAdd in checkConstraints)
                {
                    CheckConstraints.Add(new CheckConstraintBuilder(itemToAdd));
                }
            }
            return this;
        }
        public TableBuilder()
        {
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
            foreach (var x in source.PrimaryKeyConstraints ?? Enumerable.Empty<IPrimaryKeyConstraint>()) PrimaryKeyConstraints.Add(new PrimaryKeyConstraintBuilder(x));
            foreach (var x in source.UniqueConstraints ?? Enumerable.Empty<IUniqueConstraint>()) UniqueConstraints.Add(new UniqueConstraintBuilder(x));
            foreach (var x in source.Indexes ?? Enumerable.Empty<IIndex>()) Indexes.Add(new IndexBuilder(x));
            foreach (var x in source.Fields ?? Enumerable.Empty<ITableField>()) Fields.Add(new TableFieldBuilder(x));
            Name = source.Name;
            foreach (var x in source.Metadata ?? Enumerable.Empty<IMetadata>()) Metadata.Add(new MetadataBuilder(x));
            foreach (var x in source.DefaultValueConstraints ?? Enumerable.Empty<IDefaultValueConstraint>()) DefaultValueConstraints.Add(new DefaultValueConstraintBuilder(x));
            foreach (var x in source.ForeignKeyConstraints ?? Enumerable.Empty<IForeignKeyConstraint>()) ForeignKeyConstraints.Add(new ForeignKeyConstraintBuilder(x));
            foreach (var x in source.CheckConstraints ?? Enumerable.Empty<ICheckConstraint>()) CheckConstraints.Add(new CheckConstraintBuilder(x));
        }
    }
}
