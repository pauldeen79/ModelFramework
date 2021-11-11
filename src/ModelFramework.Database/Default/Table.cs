using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class Table : ITable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Table" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fileGroupName">Name of the file group.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="primaryKeyConstraints">The constraints.</param>
        /// <param name="uniqueConstraints">The unique constraints.</param>
        /// <param name="defaultValueConstraints">The default value constraints.</param>
        /// <param name="foreignKeyConstraints">The foreign key constraints.</param>
        /// <param name="indexes">The indexes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        public Table(string name
            , string fileGroupName = null
            , IEnumerable<ITableField> fields = null
            , IEnumerable<IPrimaryKeyConstraint> primaryKeyConstraints = null
            , IEnumerable<IUniqueConstraint> uniqueConstraints = null
            , IEnumerable<IDefaultValueConstraint> defaultValueConstraints = null
            , IEnumerable<IForeignKeyConstraint> foreignKeyConstraints = null
            , IEnumerable<IIndex> indexes = null
            , IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            FileGroupName = fileGroupName;
            Fields = new List<ITableField>(fields ?? Enumerable.Empty<ITableField>());
            PrimaryKeyConstraints = new List<IPrimaryKeyConstraint>(primaryKeyConstraints ?? Enumerable.Empty<IPrimaryKeyConstraint>());
            UniqueConstraints = new List<IUniqueConstraint>(uniqueConstraints ?? Enumerable.Empty<IUniqueConstraint>());
            DefaultValueConstraints = new List<IDefaultValueConstraint>(defaultValueConstraints ?? Enumerable.Empty<IDefaultValueConstraint>());
            ForeignKeyConstraints = new List<IForeignKeyConstraint>(foreignKeyConstraints ?? Enumerable.Empty<IForeignKeyConstraint>());
            Indexes = new List<IIndex>(indexes ?? Enumerable.Empty<IIndex>());
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the name of the file group.
        /// </summary>
        /// <value>
        /// The name of the file group.
        /// </value>
        public string FileGroupName { get; }

        /// <summary>
        /// Gets the primary key constraints.
        /// </summary>
        /// <value>
        /// The primary key constraints.
        /// </value>
        public IReadOnlyCollection<IPrimaryKeyConstraint> PrimaryKeyConstraints { get; }

        /// <summary>
        /// Gets the unqiue constraints.
        /// </summary>
        /// <value>
        /// The unique constraints.
        /// </value>
        public IReadOnlyCollection<IUniqueConstraint> UniqueConstraints { get; }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>
        /// The indexes.
        /// </value>
        public IReadOnlyCollection<IIndex> Indexes { get; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public IReadOnlyCollection<ITableField> Fields { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }

        /// <summary>
        /// Gets the default value constraints.
        /// </summary>
        /// <value>
        /// The default value constraints.
        /// </value>
        public IReadOnlyCollection<IDefaultValueConstraint> DefaultValueConstraints { get; }

        /// <summary>
        /// Gets the foreign key constraints.
        /// </summary>
        /// <value>
        /// The foreign key constraints.
        /// </value>
        public IReadOnlyCollection<IForeignKeyConstraint> ForeignKeyConstraints { get; }
    }
}
