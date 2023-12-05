namespace ClassFramework.TemplateFramework.Extensions;

public static class TemplateContextExtensions
{
    public static int GetIndentCount(this ITemplateContext context)
        => GetCount(context, 0);

    private static int GetCount(ITemplateContext context, int recursionLevel)
    {
        var count = context.Model is TypeBase
            ? 1
            : 0;

        if (context.ParentContext is not null)
        {
            if (recursionLevel == 24)
            {
                throw new NotSupportedException("Only 25 nested levels of sub classes are supported");
            }
            
            count += GetCount(context.ParentContext, recursionLevel + 1);
        }

        return count;
    }
}
