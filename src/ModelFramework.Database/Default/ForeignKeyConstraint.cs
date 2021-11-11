using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class ForeignKeyConstraint : IForeignKeyConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyConstraint"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="foreignTableName">Name of the foreign table.</param>
        /// <param name="localFields">The local fields.</param>
        /// <param name="foreignFields">The foreign fields.</param>
        /// <param name="cascadeUpdate">if set to <c>true</c> [cascade update].</param>
        /// <param name="cascadeDelete">if set to <c>true</c> [cascade delete].</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        /// <exception cref="ArgumentException">
        /// LocalFields should contain at least 1 value;localFields
        /// or
        /// ForeignFields should contain at least 1 value;foreignFields
        /// </exception>
        public ForeignKeyConstraint(string name, string foreignTableName, IEnumerable<IForeignKeyConstraintField> localFields, IEnumerable<IForeignKeyConstraintField> foreignFields, CascadeAction cascadeUpdate = CascadeAction.NoAction, CascadeAction cascadeDelete = CascadeAction.NoAction, IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            ForeignTableName = foreignTableName;
            LocalFields = new List<IForeignKeyConstraintField>(localFields ?? Enumerable.Empty<IForeignKeyConstraintField>());
            ForeignFields = new List<IForeignKeyConstraintField>(foreignFields ?? Enumerable.Empty<IForeignKeyConstraintField>());
            CascadeUpdate = cascadeUpdate;
            CascadeDelete = cascadeDelete;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());

            if (LocalFields?.Any() != true)
            {
                throw new ArgumentException("LocalFields should contain at least 1 value", nameof(localFields));
            }
            if (ForeignFields?.Any() != true)
            {
                throw new ArgumentException("ForeignFields should contain at least 1 value", nameof(foreignFields));
            }
        }

        /// <summary>
        /// Gets the local fields.
        /// </summary>
        /// <value>
        /// The local fields.
        /// </value>
        public IReadOnlyCollection<IForeignKeyConstraintField> LocalFields { get; }

        /// <summary>
        /// Gets the foreign fields.
        /// </summary>
        /// <value>
        /// The foreign fields.
        /// </value>
        public IReadOnlyCollection<IForeignKeyConstraintField> ForeignFields { get; }

        /// <summary>
        /// Gets the name of the foreign table.
        /// </summary>
        /// <value>
        /// The name of the foreign table.
        /// </value>
        public string ForeignTableName { get; }

        /// <summary>
        /// Gets a value indicating whether [cascade update].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cascade update]; otherwise, <c>false</c>.
        /// </value>
        public CascadeAction CascadeUpdate { get; }

        /// <summary>
        /// Gets a value indicating whether [cascade delete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cascade delete]; otherwise, <c>false</c>.
        /// </value>
        public CascadeAction CascadeDelete { get; }

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
    }
}
