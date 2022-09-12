namespace ModelFramework.Common.Extensions;

public static class StringExtensions
{
    public static string ToPascalCase(this string value)
        => string.IsNullOrEmpty(value)
            ? value
            : string.Concat(value.Substring(0, 1).ToLower(CultureInfo.InvariantCulture), value.Substring(1));

    public static string SqlEncode(this string value)
        => "'" + value.Replace("'", "''") + "'";

    public static string FixTypeName(this string? instance)
    {
        if (instance == null)
        {
            return string.Empty;
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
        => instance switch
        {
            "System.Char" => "char",
            "System.String" => "string",
            "System.Boolean" => "bool",
            "System.Object" => "object",
            "System.Decimal" => "decimal",
            "System.Double" => "double",
            "System.Single" => "float",
            "System.Byte" => "byte",
            "System.SByte" => "sbyte",
            "System.Int16" => "short",
            "System.UInt16" => "ushort",
            "System.Int32" => "int",
            "System.UInt32" => "uint",
            "System.Int64" => "long",
            "System.UInt64" => "ulong",
            _ => instance
                .ReplaceGenericArgument("System.Char", "char")
                .ReplaceGenericArgument("System.String", "string")
                .ReplaceGenericArgument("System.Boolean", "bool")
                .ReplaceGenericArgument("System.Object", "object")
                .ReplaceGenericArgument("System.Decimal", "decimal")
                .ReplaceGenericArgument("System.Double", "double")
                .ReplaceGenericArgument("System.Single", "float")
                .ReplaceGenericArgument("System.Byte", "byte")
                .ReplaceGenericArgument("System.SByte", "sbyte")
                .ReplaceGenericArgument("System.Int16", "short")
                .ReplaceGenericArgument("System.UInt16", "ushort")
                .ReplaceGenericArgument("System.Int32", "int")
                .ReplaceGenericArgument("System.UInt32", "uint")
                .ReplaceGenericArgument("System.Int64", "long")
                .ReplaceGenericArgument("System.UInt64", "ulong")
        };

    private static string ReplaceGenericArgument(this string instance, string find, string replace)
        => instance
            .Replace($"<{find}", $"<{replace}")
            .Replace($"{find}>", $"{replace}>")
            .Replace($",{find}", $",{replace}")
            .Replace($", {find}", $", {replace}")
            .Replace($"{find}[]", $"{replace}[]");

    public static string GetCsharpFriendlyName(this string instance)
        => _keywords.Contains(instance)
            ? "@" + instance
            : instance;

    public static string GetGenericArguments(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }
        //Bla<GenericArg1,...>

        var open = value.IndexOf("<");
        if (open == -1)
        {
            return string.Empty;
        }

        var comma = value.IndexOf(",", open);
        if (comma == -1)
        {
            comma = value.IndexOf(">", open);
        }

        if (comma == -1)
        {
            return string.Empty;
        }

        return value.Substring(open + 1, comma - open - 1);
    }

    public static string Sanitize(this string token)
    {
        // Replace all invalid chars by underscores 
        token = Regex.Replace(token, @"[\W\b]", "_", RegexOptions.IgnoreCase);

        // If it starts with a digit, prefix it with an underscore 
        token = Regex.Replace(token, @"^\d", @"_$0");

        return token;
    }

    public static bool IsRequiredEnum(this string instance)
        => !string.IsNullOrEmpty(instance) && Type.GetType(instance)?.IsEnum == true;

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

    public static string GetClassName(this string fullyQualifiedClassName)
    {
        var idx = fullyQualifiedClassName.LastIndexOf(".");
        return idx == -1
            ? fullyQualifiedClassName
            : fullyQualifiedClassName.Substring(idx + 1);
    }

    public static string GetNamespaceWithDefault(this string fullyQualifiedClassName, string defaultValue = "")
    {
        var idx = fullyQualifiedClassName.LastIndexOf(".");
        return idx == -1
            ? defaultValue
            : fullyQualifiedClassName.Substring(0, idx);
    }

    public static string MakeGenericTypeName(this string instance, string genericTypeParameter)
        => string.IsNullOrEmpty(genericTypeParameter)
            ? instance
            : $"{instance}<{genericTypeParameter}>";

    public static string ReplaceSuffix(this string instance, string find, string replace, StringComparison stringComparison)
    {
        var index = instance.IndexOf(find, stringComparison);
        if (index == -1 || index < instance.Length - find.Length)
        {
            return instance;
        }

        return instance.Substring(0, instance.Length - find.Length) + replace;
    }

    public static bool IsStringTypeName(this string instance)
        => instance.FixTypeName() == typeof(string).FullName;

    public static bool IsBooleanTypeName(this string instance)
        => instance.FixTypeName() == typeof(bool).FullName;

    public static bool IsNullableBooleanTypeName(this string instance)
        => instance.FixTypeName() == typeof(bool?).FullName;

    public static bool IsObjectTypeName(this string instance)
        => instance.FixTypeName() == typeof(object).FullName;

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
}
