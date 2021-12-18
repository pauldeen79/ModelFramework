using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using CrossCutting.Common.Extensions;

namespace ModelFramework.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 1)
            {
                return value;
            }

            return value.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + value.Substring(1);
        }

        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 1)
            {
                return value;
            }

            return value.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + value.Substring(1);
        }

        public static string SqlEncode(this string value)
        {
            if (value == null)
            {
                return null;
            }

            return "'" + value.Replace("'", "''") + "'";
        }

        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length < length)
            {
                return value;
            }

            return value.Substring(0, length);
        }

        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length < length)
            {
                return value;
            }

            return value.Substring(value.Length - length);
        }

        public static string WhenNull(this string value, string defaultValue = "", Action actionWhenNull = null)
        {
            if (value == null && actionWhenNull != null)
            {
                actionWhenNull();
            }

            return value ?? defaultValue;
        }

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

        public static bool? ToNullableBoolean(this string instance)
        {
            return string.IsNullOrEmpty(instance)
                ? null
                : instance.IsTrue();
        }

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

        public static bool In(this string value, IEnumerable<string> values, StringComparison stringComparison)
            => values.Any(i => i.Equals(value, stringComparison));

        public static bool In(this string value, StringComparison stringComparison, params string[] values)
            => values.Any(i => i.Equals(value, stringComparison));

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

        public static T ParseEnum<T>(this string value, T defaultValue = default)
            where T : struct
        {
            if (!Enum.TryParse(value, out T result))
            {
                return defaultValue;
            }

            return result;
        }

        public static bool ParseBooleanWithDefault(this string value, bool defaultValue = default)
        {
            if (!bool.TryParse(value, out bool result))
            {
                return defaultValue;
            }

            return result;
        }

        public static int ParseInt32WithDefault(this string value, int defaultValue = default)
        {
            if (!int.TryParse(value, out int result))
            {
                return defaultValue;
            }

            return result;
        }

        public static long ParseInt64WithDefault(this string value, long defaultValue = default)
        {
            if (!long.TryParse(value, out long result))
            {
                return defaultValue;
            }

            return result;
        }

        public static bool IsRequiredEnum(this string instance)
            => !string.IsNullOrEmpty(instance) && (Type.GetType(instance) ?? typeof(object)).IsEnum;

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

        public static string GetClassNameWithDefault(this string fullyQualifiedClassName)
        {
            var idx = fullyQualifiedClassName.LastIndexOf(".");
            return idx == -1
                ? fullyQualifiedClassName
                : fullyQualifiedClassName.Substring(idx + 1);
        }

        public static string GetNamespaceWithDefault(this string fullyQualifiedClassName, string defaultValue)
        {
            var idx = fullyQualifiedClassName.LastIndexOf(".");
            return idx == -1
                ? defaultValue
                : fullyQualifiedClassName.Substring(0, idx).WhenNullOrEmpty(defaultValue);
        }

        public static char? GetCharacterAt(this string value, int position)
            => position >= value.Length
                ? null
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
    }
}
