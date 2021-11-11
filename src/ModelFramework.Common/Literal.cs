namespace ModelFramework.Common
{
    public class Literal
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; }

        /// <summary>
        /// Gets the original value.
        /// </summary>
        /// <value>
        /// The original value.
        /// </value>
        public object OriginalValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Literal" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="originalValue">The original value.</param>
        public Literal(string value, object originalValue = null)
        {
            Value = value;
            OriginalValue = originalValue;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => Value ?? "null";
    }
}
