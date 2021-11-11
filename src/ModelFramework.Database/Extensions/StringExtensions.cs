using ModelFramework.Database.Contracts;
using System;
using System.Linq;

namespace ModelFramework.Database.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether the instance is a database string type.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static bool IsDatabaseStringType(this string instance)
            => instance != null && new[] { SqlTableFieldTypes.Char,
                                           SqlTableFieldTypes.NChar,
                                           SqlTableFieldTypes.NVarChar,
                                           SqlTableFieldTypes.VarChar }.Any(x => x.Equals(instance, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Determines whetherthe instance is a floating point numeric type.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static bool IsDatabaseFloatingPointNumericType(this string instance)
            //TODO: Review which TSQL field types allow numeric precision and scale
            => instance?.Equals(SqlTableFieldTypes.Numeric, StringComparison.OrdinalIgnoreCase) == true;

        /// <summary>
        /// Formats this string instance as database identifier.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static string FormatAsDatabaseIdentifier(this string instance)
            => instance?
                .Replace("[", string.Empty)
                .Replace("]", string.Empty);
    }
}
