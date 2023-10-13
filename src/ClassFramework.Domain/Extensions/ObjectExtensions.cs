namespace ClassFramework.Domain.Extensions;

public static class ObjectExtensions
{
    public static string CsharpFormat(this object? value, CultureInfo cultureInfo)
    {
        if (value is null)
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

        return value.ToStringWithCulture(cultureInfo);
    }

    private static string ToStringWithCulture(this object objectToConvert, CultureInfo cultureInfo)
    {
        var t = objectToConvert.GetType();
        var method = t.GetMethod("ToString", new Type[] { typeof(IFormatProvider) });
        if (method is null)
        {
            return objectToConvert.ToString();
        }
        else
        {
            return (string)method.Invoke(objectToConvert, new object[] { cultureInfo });
        }
    }
}
