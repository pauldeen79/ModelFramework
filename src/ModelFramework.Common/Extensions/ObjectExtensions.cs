using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToStringWithNullCheck(this object value)
            => value == null
                ? string.Empty
                : value.ToString();

        public static string ToStringWithDefault(this object value, string defaultValue = null)
            => value == null
                ? defaultValue
                : value.ToString();

        public static bool IsTrue(this object instance)
            => (instance is bool x && x) || instance.ToStringWithDefault().IsTrue();

        public static bool IsTrue<T>(this T instance, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return predicate(instance);
        }

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

        public static bool In<T>(this T value, IEnumerable<T> values)
            => values.Any(i => i.Equals(value));

        public static bool In<T>(this T value, params T[] values)
            => values.Any(i => i.Equals(value));

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
