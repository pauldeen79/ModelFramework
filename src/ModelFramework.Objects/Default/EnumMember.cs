using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    /// <summary>
    /// Default implementation for the IMember interface.
    /// </summary>
    public class EnumMember : IEnumMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMember" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="metadata">The metadata.</param>
        public EnumMember(string name, object value = null, IEnumerable<IAttribute> attributes = null, IEnumerable<IMetadata> metadata = null)
        {
            Name = name;
            Value = value;
            Attributes = new List<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IReadOnlyCollection<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

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

        public override string ToString() => Value != null ? $"[{Name}] = [{Value}]" : Name;
    }
}
