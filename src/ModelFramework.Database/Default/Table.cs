using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record Table : ITable
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public Table(string name,
                     string fileGroupName,
                     IEnumerable<ITableField> fields,
                     IEnumerable<IPrimaryKeyConstraint> primaryKeyConstraints,
                     IEnumerable<IUniqueConstraint> uniqueConstraints,
                     IEnumerable<IDefaultValueConstraint> defaultValueConstraints,
                     IEnumerable<IForeignKeyConstraint> foreignKeyConstraints,
                     IEnumerable<IIndex> indexes,
                     IEnumerable<ICheckConstraint> checkConstraints,
                     IEnumerable<IMetadata> metadata)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            FileGroupName = fileGroupName;
            Fields = new ValueCollection<ITableField>(fields);
            PrimaryKeyConstraints = new ValueCollection<IPrimaryKeyConstraint>(primaryKeyConstraints);
            UniqueConstraints = new ValueCollection<IUniqueConstraint>(uniqueConstraints);
            DefaultValueConstraints = new ValueCollection<IDefaultValueConstraint>(defaultValueConstraints);
            ForeignKeyConstraints = new ValueCollection<IForeignKeyConstraint>(foreignKeyConstraints);
            Indexes = new ValueCollection<IIndex>(indexes);
            CheckConstraints = new ValueCollection<ICheckConstraint>(checkConstraints);
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public string FileGroupName { get; }
        public ValueCollection<IPrimaryKeyConstraint> PrimaryKeyConstraints { get; }
        public ValueCollection<IUniqueConstraint> UniqueConstraints { get; }
        public ValueCollection<IIndex> Indexes { get; }
        public ValueCollection<ITableField> Fields { get; }
        public string Name { get; }
        public ValueCollection<IDefaultValueConstraint> DefaultValueConstraints { get; }
        public ValueCollection<IForeignKeyConstraint> ForeignKeyConstraints { get; }
        public ValueCollection<ICheckConstraint> CheckConstraints { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
