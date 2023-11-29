namespace ClassFramework.Domain.Extensions;

public static class GenericTypeArgumentsContainerExtensions
{
    public static string GetGenericTypeArgumentsString(this IGenericTypeArgumentsContainer instance)
        => instance.GenericTypeArguments.Count > 0
            ? $"<{string.Join(", ", instance.GenericTypeArguments)}>"
            : string.Empty;
}
