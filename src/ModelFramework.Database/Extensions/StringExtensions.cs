using System;
using System.Linq;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Extensions
{
    public static class StringExtensions
    {
        public static bool IsDatabaseStringType(this string instance)
            => instance != null && new[] { SqlTableFieldTypes.Char,
                                           SqlTableFieldTypes.NChar,
                                           SqlTableFieldTypes.NVarChar,
                                           SqlTableFieldTypes.VarChar }.Any(x => x.Equals(instance, StringComparison.OrdinalIgnoreCase));

        public static bool IsDatabaseFloatingPointNumericType(this string instance)
            //TODO: Review which TSQL field types allow numeric precision and scale
            => instance?.Equals(SqlTableFieldTypes.Numeric, StringComparison.OrdinalIgnoreCase) == true;

        public static string FormatAsDatabaseIdentifier(this string instance)
            => instance?
                .Replace("[", string.Empty)
                .Replace("]", string.Empty);
    }
}
