﻿namespace ModelFramework.Database.Extensions;

public static class StringExtensions
{
    private static readonly string[] _databaseStringTypes =
    [
        SqlTableFieldTypes.Char,
        SqlTableFieldTypes.NChar,
        SqlTableFieldTypes.NVarChar,
        SqlTableFieldTypes.VarChar
    ];

    public static bool IsDatabaseStringType(this string instance)
        => Array.Exists(_databaseStringTypes, x => x.Equals(instance, StringComparison.OrdinalIgnoreCase));

    public static string FormatAsDatabaseIdentifier(this string instance)
        => instance
            .Replace("[", string.Empty)
            .Replace("]", string.Empty);
}
