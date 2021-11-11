using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Extensions
{
    public static class IViewConditionExtensions
    {
        /// <summary>
        /// Converts the IViewCondition to an item which acts as first item in a series of conditions. (in other words, without combination)
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static IViewCondition AsFirstCondition(this IViewCondition instance)
        {
            return instance == null
                ? null
                : new ViewCondition(null, instance.Expression, instance.FileGroupName, instance.Metadata);
        }
    }
}
