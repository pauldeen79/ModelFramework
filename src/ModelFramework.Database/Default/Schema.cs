using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class Schema : ISchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Schema" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="tables">The tables.</param>
        /// <param name="storedProcedures">The stored procedures.</param>
        /// <param name="views">The views.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        public Schema(string name
            , IEnumerable<ITable> tables = null
            , IEnumerable<IStoredProcedure> storedProcedures = null
            , IEnumerable<IView> views = null
            , IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Tables = new List<ITable>(tables ?? Enumerable.Empty<ITable>());
            StoredProcedures = new List<IStoredProcedure>(storedProcedures ?? Enumerable.Empty<IStoredProcedure>());
            Views = new List<IView>(views ?? Enumerable.Empty<IView>());
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the tables.
        /// </summary>
        /// <value>
        /// The tables.
        /// </value>
        public IReadOnlyCollection<ITable> Tables { get; }

        /// <summary>
        /// Gets the stored procedures.
        /// </summary>
        /// <value>
        /// The stored procedures.
        /// </value>
        public IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; }

        /// <summary>
        /// Gets the views.
        /// </summary>
        /// <value>
        /// The views.
        /// </value>
        public IReadOnlyCollection<IView> Views { get; }

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
