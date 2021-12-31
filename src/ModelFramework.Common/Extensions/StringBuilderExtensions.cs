using System.Text;

namespace ModelFramework.Common.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AddWithCondition(this StringBuilder builder, object? value, bool condition)
        {
            if (!condition)
            {
                return builder;
            }
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return builder;
            }
            if (builder.Length > 0)
            {
                builder.Append(" ");
            }
            return builder.Append(value);
        }
    }
}
