using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class ViewOrderByField : IViewOrderByField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewOrderByField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sourceSchemaName">Name of the source schema.</param>
        /// <param name="sourceObjectName">Name of the source object.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="descending">if set to <c>true</c> [descending].</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name - Name cannot be null or whitespace</exception>
        public ViewOrderByField(
            string name,
            string sourceSchemaName = null,
            string sourceObjectName = null,
            string expression = null,
            string alias = null,
            bool descending = false,
            IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            SourceSchemaName = sourceSchemaName;
            SourceObjectName = sourceObjectName;
            Expression = expression;
            Alias = alias;
            Descending = descending;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ViewOrderByField"/> is descending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if descending; otherwise, <c>false</c>.
        /// </value>
        public bool Descending { get; }

        /// <summary>
        /// Gets the name of the source schema.
        /// </summary>
        /// <value>
        /// The name of the source schema.
        /// </value>
        public string SourceSchemaName { get; }

        /// <summary>
        /// Gets the name of the source object.
        /// </summary>
        /// <value>
        /// The name of the source object.
        /// </value>
        public string SourceObjectName { get; }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        public string Expression { get; }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; }

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
