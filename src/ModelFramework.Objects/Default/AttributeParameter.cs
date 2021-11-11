using ModelFramework.Common.Contracts;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    /// <summary>
    /// Default implementation for the IAttributeParameter interface.
    /// </summary>
    public class AttributeParameter : IAttributeParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeParameter" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">value;Value cannot be null</exception>
        public AttributeParameter(object value, string name = null, IEnumerable<IMetadata> metadata = null)
        {
            Name = name;
            Value = value ?? throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be null");
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        public override string ToString() => $"{Name.WhenNullOrEmpty(Value.ToStringWithDefault(string.Empty))}";
    }
}
