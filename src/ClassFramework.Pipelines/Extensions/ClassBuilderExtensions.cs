namespace ClassFramework.Pipelines.Extensions;

public static class ClassBuilderExtensions
{
    public static string FormatInstanceName(
        this ClassBuilder instance,
        bool forCreate,
        Func<ClassBuilder, bool, string>? formatInstanceTypeNameDelegate)
    {
        if (formatInstanceTypeNameDelegate is not null)
        {
            var retVal = formatInstanceTypeNameDelegate(instance, forCreate);
            if (!string.IsNullOrEmpty(retVal))
            {
                return retVal;
            }
        }

        return instance.IsNotNull(nameof(instance)).GetFullName().GetCsharpFriendlyTypeName();
    }
}
