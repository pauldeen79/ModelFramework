using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class TableFieldCheckConstraint : ITableFieldCheckConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        public TableFieldCheckConstraint
            (
            string name,
            string expression,
            IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            if (string.IsNullOrWhiteSpace(expression)) throw new ArgumentOutOfRangeException(nameof(expression), "Expression cannot be null or whitespace");

            Name = name;
            Expression = expression;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public string Expression { get; }

        public string Name { get; }

        public IReadOnlyCollection<IMetadata> Metadata { get; }
    }
}
