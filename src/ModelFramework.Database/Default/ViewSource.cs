using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class ViewSource : IViewSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewSource" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="sourceSchemaName">Name of the source schema.</param>
        /// <param name="sourceObjectName">Name of the source object.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name - Name cannot be null or whitespace</exception>
        public ViewSource(string name
            , string alias = null
            , string sourceSchemaName = null
            , string sourceObjectName = null
            , IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Alias = alias;
            SourceSchemaName = sourceSchemaName;
            SourceObjectName = sourceObjectName;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

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
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }
    }
}
