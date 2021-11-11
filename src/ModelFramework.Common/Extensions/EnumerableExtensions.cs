using System;
using System.Collections.Generic;

namespace ModelFramework.Common.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Gets the value of an IEnumerable, or a default value when it's empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="valueWhenNull">The value when null. If this is null, an empty array will be used.</param>
        /// <returns>
        /// Typed array.
        /// </returns>
        public static IEnumerable<T> DefaultWhenNull<T>(this IEnumerable<T> instance, IEnumerable<T> valueWhenNull = null) =>
            instance ?? valueWhenNull ?? Array.Empty<T>();

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
