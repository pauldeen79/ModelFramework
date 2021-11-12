using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Extensions
{
    public static class IViewConditionExtensions
    {
        public static IViewCondition AsFirstCondition(this IViewCondition instance)
            => instance == null
                ? null
                : new ViewCondition(null, instance.Expression, instance.FileGroupName, instance.Metadata);
    }
}
