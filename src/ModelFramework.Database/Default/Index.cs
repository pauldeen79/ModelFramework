using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class Index : IIndex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Index" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="unique">if set to <c>true</c> [unique].</param>
        /// <param name="fields">The fields.</param>
        /// <param name="fileGroupName">Name of the file group.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        /// <exception cref="ArgumentException">Fields should contain at least 1 value;fields</exception>
        public Index(string name, bool unique, IEnumerable<IIndexField> fields, string fileGroupName = null, IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            if (fields?.Any() != true)
            {
                throw new ArgumentException("Fields should contain at least 1 value", nameof(fields));
            }

            Name = name;
            Unique = unique;
            FileGroupName = fileGroupName;
            Fields = new List<IIndexField>(fields);
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public IReadOnlyCollection<IIndexField> Fields { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Index"/> is unique.
        /// </summary>
        /// <value>
        ///   <c>true</c> if unique; otherwise, <c>false</c>.
        /// </value>
        public bool Unique { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the name of the file group.
        /// </summary>
        /// <value>
        /// The name of the file group.
        /// </value>
        public string FileGroupName { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }
    }
}
