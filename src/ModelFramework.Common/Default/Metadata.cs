using ModelFramework.Common.Contracts;
using System;

namespace ModelFramework.Common.Default
{
    /// <summary>
    /// Default implementation for the IMetadata interface.
    /// </summary>
    public class Metadata : IMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Metadata"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        public Metadata(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        public override string ToString() => $"[{Name}] = [{Value}]";
    }
}
