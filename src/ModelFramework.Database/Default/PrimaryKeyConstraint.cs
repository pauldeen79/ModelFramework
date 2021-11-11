using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class PrimaryKeyConstraint : IPrimaryKeyConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyConstraint" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fileGroupName">Name of the file group.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        /// <exception cref="ArgumentException">Fields should contain at least 1 value;fields</exception>
        public PrimaryKeyConstraint(string name, string fileGroupName = null, IEnumerable<IPrimaryKeyConstraintField> fields = null, IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            if (fields?.Any() != true)
            {
                throw new ArgumentException("Fields should contain at least 1 value", nameof(fields));
            }

            Name = name;
            FileGroupName = fileGroupName;
            Fields = new List<IPrimaryKeyConstraintField>(fields);
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

        /// <summary>
        /// Gets the name of the file group.
        /// </summary>
        /// <value>
        /// The name of the file group.
        /// </value>
        public string FileGroupName { get; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public IReadOnlyCollection<IPrimaryKeyConstraintField> Fields { get; }
    }
}
