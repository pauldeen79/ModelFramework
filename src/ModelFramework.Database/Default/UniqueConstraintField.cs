using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class UniqueConstraintField : IUniqueConstraintField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueConstraintField" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        public UniqueConstraintField(string name, IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

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
