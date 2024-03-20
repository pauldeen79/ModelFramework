namespace DatabaseFramework.Database.Extensions;

public static class StringExtensions
{
    public static string FormatAsDatabaseIdentifier(this string instance)
        => instance
            .Replace("[", string.Empty)
            .Replace("]", string.Empty);
}
