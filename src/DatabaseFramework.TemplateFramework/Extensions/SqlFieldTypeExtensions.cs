namespace DatabaseFramework.TemplateFramework.Extensions;

public static class SqlFieldTypeExtensions
{
    private static readonly SqlFieldType[] _databaseStringTypes =
    [
        SqlFieldType.Char,
        SqlFieldType.NChar,
        SqlFieldType.NVarChar,
        SqlFieldType.VarChar
    ];

    public static bool IsDatabaseStringType(this SqlFieldType instance)
        => Array.Exists(_databaseStringTypes, x => x == instance);
}
