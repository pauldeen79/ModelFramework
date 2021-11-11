using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelFramework.Common.Extensions
{
    /// <summary>
    /// Class which contains extension methods for the String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to camel case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// camel cased string (first character upper case, remaining characters unchanged)
        /// </returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 1)
            {
                return value;
            }

            return value.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + value.Substring(1);
        }

        /// <summary>
        /// Converts a string to pascal case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// pascal cased string (first character lower case, remaining characters unchanged)
        /// </returns>
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 1)
            {
                return value;
            }

            return value.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + value.Substring(1);
        }

        /// <summary>
        /// SQL encodes the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL encoded string. ('{value with ' replaced by ''}])
        /// </returns>
        public static string SqlEncode(this string value)
        {
            if (value == null)
            {
                return null;
            }

            return "'" + value.Replace("'", "''") + "'";
        }

        /// <summary>
        /// Returns the first number of characters of a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length < length)
            {
                return value;
            }

            return value.Substring(0, length);
        }

        /// <summary>
        /// Returns the last number of characters of a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length < length)
            {
                return value;
            }

            return value.Substring(value.Length - length);
        }

        /// <summary>
        /// Returns a default value when the string value is empty, or otherwise the current string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="actionWhenNull">Optional action to perform when the value is null.</param>
        /// <returns>
        /// Default value when null, otherwise the current value.
        /// </returns>
        public static string WhenNull(this string value, string defaultValue = "", Action actionWhenNull = null)
        {
            if (value == null && actionWhenNull != null)
            {
                actionWhenNull();
            }

            return value ?? defaultValue;
        }

        /// <summary>
        /// Fixes the name of the type.
        /// </summary>
        /// <param name="instance">The type name to fix.</param>
        /// <returns></returns>
        public static string FixTypeName(this string instance)
        {
            if (instance == null)
            {
                return null;
            }

            int startIndex;
            while (true)
            {
                startIndex = instance.IndexOf(", ");
                if (startIndex == -1)
                {
                    break;
                }

                int secondIndex = instance.IndexOf("]", startIndex + 1);
                if (secondIndex == -1)
                {
                    break;
                }

                instance = instance.Substring(0, startIndex) + instance.Substring(secondIndex + 1);
            }

            while (true)
            {
                startIndex = instance.IndexOf("`");
                if (startIndex == -1)
                {
                    break;
                }

                instance = instance.Substring(0, startIndex) + instance.Substring(startIndex + 2);
            }

            //remove assebmly name from type, e.g. System.String, mscorlib bla bla bla -> System.String
            var index = instance.IndexOf(", ");
            if (index > -1)
            {
                instance = instance.Substring(0, index);
            }

            return FixAnonymousTypeName(instance
                .Replace("[[", "<")
                .Replace(",[", ",")
                .Replace(",]", ">")
                .Replace("]", ">")
                .Replace("[>", "[]") //hacking here! caused by the previous statements...
                .Replace("System.Void", "void")
                .Replace("+", ".")
                .Replace("&", ""));
        }

        public static string GetCsharpFriendlyTypeName(this string instance)
        {
            if (string.IsNullOrEmpty(instance))
            {
                return instance;
            }

            return instance
                .Replace("System.Char", "char")
                .Replace("System.String", "string")
                .Replace("System.Boolean", "bool")
                .Replace("System.Object", "object")
                .Replace("System.Decimal", "decimal")
                .Replace("System.Double", "double")
                .Replace("System.Single", "float")
                .Replace("System.Byte", "byte")
                .Replace("System.SByte", "sbyte")
                .Replace("System.Int16", "short")
                .Replace("System.UInt16", "ushort")
                .Replace("System.Int32", "int")
                .Replace("System.UInt32", "uint")
                .Replace("System.Int64", "long")
                .Replace("System.UInt64", "ulong");
        }

        private static readonly string[] _keywords = new[]
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "void",
            "volatile",
            "while"
        };

        public static string GetCsharpFriendlyName(this string instance)
        {
            if (_keywords.Contains(instance ?? string.Empty))
            {
                return "@" + instance;
            }

            return instance;
        }

        private static string FixAnonymousTypeName(string instance)
        {
            var isAnonymousType = instance.Contains("AnonymousType")
                && (instance.Contains("<>") || instance.Contains("VB$"));

            var arraySuffix = instance.EndsWith("[]")
                ? "[]"
                : string.Empty;

            return isAnonymousType
                ? $"AnonymousType{arraySuffix}"
                : instance;
        }

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetGenericArguments(this string value)
        {
            var fixedTypeName = value.FixTypeName();
            if (string.IsNullOrEmpty(fixedTypeName))
            {
                return null;
            }
            //Bla<GenericArg1,...>

            var open = fixedTypeName.IndexOf("<");
            if (open == -1)
            {
                return null;
            }

            var comma = fixedTypeName.IndexOf(",", open);
            if (comma == -1)
            {
                comma = fixedTypeName.IndexOf(">", open);
            }

            if (comma == -1)
            {
                return null;
            }

            return fixedTypeName.Substring(open + 1, comma - open - 1);
        }

        private static readonly string[] _trueKeywords = new[] { "true", "t", "1", "y", "yes", "ja", "j"};
        private static readonly string[] _falseKeywords = new[] { "false", "f", "0", "n", "no", "nee" };

        /// <summary>
        /// Determines whether the specified instance is true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsTrue(this string instance)
        {
            return instance != null && _trueKeywords.Any(s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines whether the specified instance is false.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsFalse(this string instance)
        {
            return instance != null && _falseKeywords.Any(s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Converts the string instance to a nullable boolean.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// null when empty, true when the instance is any value representing true, or otherwise false.
        /// </returns>
        public static bool? ToNullableBoolean(this string instance)
        {
            return string.IsNullOrEmpty(instance)
                ? (bool?)null
                : instance.IsTrue();
        }

        /// <summary>
        /// Formats the specified string
        /// </summary>
        /// <param name="str">The string to format.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string Format(this string str, params Expression<Func<string, object[], object>>[] args)
        {
            var parameters = args.ToDictionary(e => string.Format("{{{0}}}", e.Parameters[0].Name), e => e.Compile()(e.Parameters[0].Name, args));

            var sb = new StringBuilder(str);
            foreach (var kv in parameters)
            {
                sb.Replace(kv.Key, kv.Value != null ? kv.Value.ToString() : "");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Performs a is null or empty check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrEmpty">The when null or empty.</param>
        /// <returns></returns>
        public static string WhenNullOrEmpty(this string instance, string whenNullOrEmpty)
        {
            if (string.IsNullOrEmpty(instance))
            {
                return whenNullOrEmpty;
            }

            return instance;
        }

        /// <summary>
        /// Performs a is null or empty check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrEmptyDelegate">The delegate to invoke when null or empty.</param>
        /// <returns></returns>
        public static string WhenNullOrEmpty(this string instance, Func<string> whenNullOrEmptyDelegate)
        {
            if (string.IsNullOrEmpty(instance))
            {
                if (whenNullOrEmptyDelegate == null)
                {
                    throw new ArgumentNullException(nameof(whenNullOrEmptyDelegate));
                }

                return whenNullOrEmptyDelegate();
            }

            return instance;
        }

        /// <summary>
        /// Performs a is null or whitespace check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrWhiteSpace">The when null or white space.</param>
        /// <returns></returns>
        public static string WhenNullOrWhitespace(this string instance, string whenNullOrWhiteSpace)
        {
            if (string.IsNullOrWhiteSpace(instance))
            {
                return whenNullOrWhiteSpace;
            }

            return instance;
        }

        /// <summary>
        /// Performs a is null or whitespace check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrWhiteSpaceDelegate">The when null or white space.</param>
        /// <returns></returns>
        public static string WhenNullOrWhitespace(this string instance, Func<string> whenNullOrWhiteSpaceDelegate)
        {
            if (string.IsNullOrWhiteSpace(instance))
            {
                if (whenNullOrWhiteSpaceDelegate == null)
                {
                    throw new ArgumentNullException(nameof(whenNullOrWhiteSpaceDelegate));
                }

                return whenNullOrWhiteSpaceDelegate();
            }

            return instance;
        }

        /// <summary>
        /// Determines whether the specified value is contained within the specified sequence.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <param name="values">The sequence to search in.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <returns>
        /// true when found, otherwise false.
        /// </returns>
        public static bool In(this string value, IEnumerable<string> values, StringComparison stringComparison)
        {
            return values.Any(i => i.Equals(value, stringComparison));
        }

        /// <summary>
        /// Determines whether the specified value is contained within the specified sequence.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <param name="values">The sequence to search in.</param>
        /// <returns>
        /// true when found, otherwise false.
        /// </returns>
        public static bool In(this string value, StringComparison stringComparison, params string[] values)
        {
            return values.Any(i => i.Equals(value, stringComparison));
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified Unicode character in this instance are replaced with another specified Unicode character.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            if (newValue == null)
            {
                throw new ArgumentNullException(nameof(newValue));
            }

            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str, previousIndex, index - previousIndex);
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str, previousIndex, str.Length - previousIndex);

            return sb.ToString();
        }

        /// <summary>
        /// Encodes the XML value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="allowNullEncoding">if set to <c>true</c> [allow null encoding].</param>
        /// <returns></returns>
        /// <remarks>
        /// Currently, ASCII 0 through 31 (except 9, 10 ad 13) are replaced with a question mark.
        /// </remarks>
        public static string EncodeXmlValue(this string value, bool allowNullEncoding = false)
        {
            if (value == null)
            {
                return allowNullEncoding
                    ? "{xsi:nil}"
                    : string.Empty;
            }

            //replace ascii 0 t/m 31 (except 9, 10 and 13) with a question mark. this fixes the exception "'.', hexadecimal value 0x00, is an invalid character."
            var invalidList = new List<int>();
            for (int i = 0; i <= 31; i++)
            {
                if (i == 9 || i == 10 || i == 13)
                {
                    continue;
                }

                invalidList.Add(i);
            }
            return invalidList.Aggregate(value, (current, item) => current.Replace(((char)item).ToString(CultureInfo.InvariantCulture), "?"));
        }

        /// <summary>
        /// Sanitizes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static string Sanitize(this string token)
        {
            if (token == null)
            {
                return string.Empty;
            }

            // Replace all invalid chars by underscores 
            token = Regex.Replace(token, @"[\W\b]", "_", RegexOptions.IgnoreCase);

            // If it starts with a digit, prefix it with an underscore 
            token = Regex.Replace(token, @"^\d", @"_$0"); 

            return token;
        }

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, params string[] values) =>
            instance.StartsWithAny((IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, IEnumerable<string> values) =>
            values.Any(v => instance.StartsWith(v));

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, StringComparison comparisonType, params string[] values) =>
            instance.StartsWithAny(comparisonType, (IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, StringComparison comparisonType, IEnumerable<string> values) =>
            values.Any(v => instance.StartsWith(v, comparisonType));

        /// <summary>
        /// Indicates whether the string instance ends with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, params string[] values) =>
            instance.EndsWithAny((IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance ends with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, IEnumerable<string> values) =>
            values.Any(v => instance.EndsWith(v));

        /// <summary>
        /// Indicates whether the string instance ends any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, StringComparison comparisonType, params string[] values) =>
            instance.EndsWithAny(comparisonType, (IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance ends any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, StringComparison comparisonType, IEnumerable<string> values) =>
            values.Any(v => instance.EndsWith(v, comparisonType));

        /// <summary>
        /// Parses the value as enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T ParseEnum<T>(this string value, T defaultValue = default)
            where T : struct
        {
            if (!Enum.TryParse(value, out T result))
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Parses the value as boolean with default value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static bool ParseBooleanWithDefault(this string value, bool defaultValue = default)
        {
            if (!bool.TryParse(value, out bool result))
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Parses the value as Int32 with default value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static int ParseInt32WithDefault(this string value, int defaultValue = default)
        {
            if (!int.TryParse(value, out int result))
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Parses the value as Int64 with default value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static long ParseInt64WithDefault(this string value, long defaultValue = default)
        {
            if (!long.TryParse(value, out long result))
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified instance is a required enumeration.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// false when instance is null or empty, or the instance is not an enumeration type, otherwise true.
        /// </returns>
        public static bool IsRequiredEnum(this string instance)
        {
            return !string.IsNullOrEmpty(instance) && (Type.GetType(instance) ?? typeof(object)).IsEnum;
        }

        /// <summary>
        /// Determines whether the specified instance is an optional enumeration.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// false when instance is null or empty, or the instance is not a nullable enumeration type, otherwise true.
        /// </returns>
        public static bool IsOptionalEnum(this string instance)
        {
            if (string.IsNullOrEmpty(instance))
            {
                return false;
            }

            var t = Type.GetType(instance);
            if (t == null)
            {
                return false;
            }

            var u = Nullable.GetUnderlyingType(t);
            return u?.IsEnum == true;
        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <param name="fullyQualifiedClassName">Fully qualified class name.</param>
        /// <returns></returns>
        public static string GetClassNameWithDefault(this string fullyQualifiedClassName)
        {
            var idx = fullyQualifiedClassName.LastIndexOf(".");
            return idx == -1
                ? fullyQualifiedClassName
                : fullyQualifiedClassName.Substring(idx + 1);
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <param name="fullyQualifiedClassName">Fully qualified class name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetNamespaceWithDefault(this string fullyQualifiedClassName, string defaultValue)
        {
            var idx = fullyQualifiedClassName.LastIndexOf(".");
            return idx == -1
                ? defaultValue
                : fullyQualifiedClassName.Substring(0, idx).WhenNullOrEmpty(defaultValue);
        }

        public static char? GetCharacterAt(this string value, int position) => position >= value.Length
                ? (char?)null
                : value[position];

        public static string RemoveSuffix(this string instance, string suffix)
        {
            if (suffix == null)
            {
                throw new ArgumentNullException(nameof(suffix));
            }

            return instance.EndsWith(suffix)
                ? instance.Substring(0, instance.Length - suffix.Length)
                : instance;
        }

        public static int OccurencesOf(this string instance, string textToFind)
        {
            int result = 0;
            int previous = -1;

            while (true)
            {
                int index = instance.IndexOf(textToFind, previous + 1);
                if (index == -1)
                {
                    break;
                }
                else
                {
                    result++;
                    previous = index;
                }
            }

            return result;
        }

        public static int OccurencesOf(this string instance, string textToFind, StringComparison stringComparison)
        {
            int result = 0;
            int previous = -1;

            while (true)
            {
                int index = instance.IndexOf(textToFind, previous + 1, stringComparison);
                if (index == -1)
                {
                    break;
                }
                else
                {
                    result++;
                    previous = index;
                }
            }

            return result;
        }

        public static string MakeGenericTypeName(this string instance, string genericTypeParameter)
            => string.IsNullOrEmpty(genericTypeParameter)
                ? instance
                : $"{instance}<{genericTypeParameter}>";

        public static bool IsUnitTestAssembly(this string instance)
            => instance.StartsWithAny(StringComparison.OrdinalIgnoreCase,
                                      "Microsoft.VisualStudio",
                                      "Microsoft.TestPlatform",
                                      "xunit",
                                      "testhost",
                                      "msdia140");
    }
}
