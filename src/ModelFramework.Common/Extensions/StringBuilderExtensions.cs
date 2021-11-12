using System.Text;

namespace ModelFramework.Common.Extensions
{
    public static class StringBuilderExtensions
    {
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
