using System;
using System.Text;

namespace ModelFramework.Common.Extensions
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Adds the specified value to the stringbuilder on the specified condition.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">The value.</param>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        public static void AddWithCondition(this StringBuilder builder, object value, bool condition = true)
        {
            if (!condition)
            {
                return;
            }
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return;
            }
            if (builder.Length > 0)
            {
                builder.Append(" ");
            }
            builder.Append(value);
        }
    }
}
