using System;
using System.Collections.Generic;

namespace ModelFramework.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DefaultWhenNull<T>(this IEnumerable<T> instance, IEnumerable<T> valueWhenNull = null)
            => instance ?? valueWhenNull ?? Array.Empty<T>();

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> instance, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in instance)
            {
                action(item);
            }

            return instance;
        }
    }
}
