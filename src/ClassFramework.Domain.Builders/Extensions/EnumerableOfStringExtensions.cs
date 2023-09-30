namespace ClassFramework.Domain.Builders.Extensions;

public static class EnumerableOfStringExtensions
{
    public static IEnumerable<StringCodeStatementBuilder> ToStringCodeStatementBuilders(this IEnumerable<string> instance)
        => instance.Select(s => new StringCodeStatementBuilder(s));
}
