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
                     string fileGroupName = null,
                     IEnumerable<ITableField> fields = null,
                     IEnumerable<IPrimaryKeyConstraint> primaryKeyConstraints = null,
                     IEnumerable<IUniqueConstraint> uniqueConstraints = null,
                     IEnumerable<IDefaultValueConstraint> defaultValueConstraints = null,
                     IEnumerable<IForeignKeyConstraint> foreignKeyConstraints = null,
                     IEnumerable<IIndex> indexes = null,
                     IEnumerable<ICheckConstraint> checkConstraints = null,
                     IEnumerable<IMetadata> metadata = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            FileGroupName = fileGroupName;
            Fields = new ValueCollection<ITableField>(fields ?? Enumerable.Empty<ITableField>());
            PrimaryKeyConstraints = new ValueCollection<IPrimaryKeyConstraint>(primaryKeyConstraints ?? Enumerable.Empty<IPrimaryKeyConstraint>());
            UniqueConstraints = new ValueCollection<IUniqueConstraint>(uniqueConstraints ?? Enumerable.Empty<IUniqueConstraint>());
            DefaultValueConstraints = new ValueCollection<IDefaultValueConstraint>(defaultValueConstraints ?? Enumerable.Empty<IDefaultValueConstraint>());
            ForeignKeyConstraints = new ValueCollection<IForeignKeyConstraint>(foreignKeyConstraints ?? Enumerable.Empty<IForeignKeyConstraint>());
            Indexes = new ValueCollection<IIndex>(indexes ?? Enumerable.Empty<IIndex>());
            CheckConstraints = new ValueCollection<ICheckConstraint>(checkConstraints ?? Enumerable.Empty<ICheckConstraint>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
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
