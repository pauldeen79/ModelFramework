using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts an object value to string with null check.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// value.ToString() when the value is not null, string.Empty otherwise.
        /// </returns>
        public static string ToStringWithNullCheck(this object value) =>
            value == null
                ? string.Empty
                : value.ToString();

        /// <summary>
        /// Converts an object value to string with default value if null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// value.ToString() when te value is not null, defaultValue otherwise.
        /// </returns>
        public static string ToStringWithDefault(this object value, string defaultValue = null) =>
            value == null
                ? defaultValue
                : value.ToString();

        /// <summary>
        /// Determines whether the specified instance is true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsTrue(this object instance) =>
            (instance is bool x && x) || instance.ToStringWithDefault().IsTrue();

        public static bool IsTrue<T>(this T instance, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return predicate(instance);
        }

        /// <summary>
        /// Determines whether the specified instance is false.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsFalse(this object instance)
            => (instance is bool x && !x) || instance.ToStringWithDefault().IsFalse();

        public static bool IsFalse<T>(this T instance, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return !predicate(instance);
        }

        /// <summary>
        /// Determines whether the specified value is contained within the specified sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to search for.</param>
        /// <param name="values">The sequence to search in.</param>
        /// <returns>
        /// true when found, otherwise false.
        /// </returns>
        public static bool In<T>(this T value, IEnumerable<T> values)
            => values.Any(i => i.Equals(value));

        /// <summary>
        /// Determines whether the specified value is contained within the specified sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to search for.</param>
        /// <param name="values">The sequence to search in.</param>
        /// <returns>
        /// true when found, otherwise false.
        /// </returns>
        public static bool In<T>(this T value, params T[] values)
            => values.Any(i => i.Equals(value));

        /// <summary>
        /// Formats the value as a CSharp string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Csharp formatted value.
        /// </returns>
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
