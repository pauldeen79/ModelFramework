namespace ModelFramework.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string CsharpFormat(this object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is string x)
            {
                return "@\"" + (x).Replace("\"", "\"\"") + "\"";
            }

            if (value is bool x2)
            {
                return (x2)
                    ? "true"
                    : "false";
            }

            return value.ToString();
        }
    }
}
