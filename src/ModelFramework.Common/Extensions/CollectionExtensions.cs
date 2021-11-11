using System.Collections.Generic;

namespace ModelFramework.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> instance, IEnumerable<T> range)
        {
            range.ForEach(instance.Add);

            return instance;
        }
    }
}
