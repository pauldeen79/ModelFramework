namespace ModelFramework.Database.Extensions;

public static class ViewConditionExtensions
{
    public static IViewCondition AsFirstCondition(this IViewCondition instance)
        => new ViewCondition(instance.Expression, string.Empty, instance.Metadata, instance.FileGroupName);
}
