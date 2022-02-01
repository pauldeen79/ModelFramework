namespace ModelFramework.Common.Extensions;

public static class ObjectExtensions
{
    public static string CsharpFormat(this object? value)
    {
        if (value == null)
        {
            return "null";
        }

        if (value is string s)
        {
            return "@\"" + (s).Replace("\"", "\"\"") + "\"";
        }

        if (value is bool b)
        {
            return b
                ? "true"
                : "false";
        }

        return value.ToStringWithCulture();
    }

    private static string ToStringWithCulture(this object objectToConvert)
    {
        var t = objectToConvert.GetType();
        var method = t.GetMethod("ToString", new Type[] { typeof(IFormatProvider) });
        if (method == null)
        {
            return objectToConvert.ToString();
        }
        else
        {
            return (string)method.Invoke(objectToConvert, new object[] { CultureInfo.InvariantCulture });
        }
    }
}
