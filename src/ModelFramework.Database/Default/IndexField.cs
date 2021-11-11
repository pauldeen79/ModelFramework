using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class IndexField : IIndexField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexField" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        public IndexField(string name, bool isDescending = false, IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            IsDescending = isDescending;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets a value indicating whether this instance is descending.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is descending; otherwise, <c>false</c>.
        /// </value>
        public bool IsDescending { get; }

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
