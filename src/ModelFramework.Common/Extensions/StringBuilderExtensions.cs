namespace ModelFramework.Common.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AddWithCondition(this StringBuilder builder, string value, bool condition)
    {
        if (!condition)
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
