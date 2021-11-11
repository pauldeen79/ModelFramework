using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class DefaultValueConstraint : IDefaultValueConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueConstraint" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="name">The name.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// name;Name cannot be null or whitespace
        /// or
        /// fieldName;FieldName cannot be null or whitespace
        /// or
        /// defaultValue;DefaultValue cannot be null or whitespace
        /// </exception>
        public DefaultValueConstraint(string fieldName, string defaultValue, string name, IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            if (string.IsNullOrWhiteSpace(fieldName)) throw new ArgumentOutOfRangeException(nameof(fieldName), "FieldName cannot be null or whitespace");
            if (string.IsNullOrWhiteSpace(defaultValue)) throw new ArgumentOutOfRangeException(nameof(defaultValue), "DefaultValue cannot be null or whitespace");

            FieldName = fieldName;
            DefaultValue = defaultValue;
            Name = name;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public string DefaultValue { get; }

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
