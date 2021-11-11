using ModelFramework.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Common.Extensions
{
    public static class EnumerableOfIMetadataExtensions
    {
        /// <summary>
        /// Gets the metadata value as a string.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="metadataName">Name of the metadata.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="convertFunction">The convert function.</param>
        public static string GetMetadataStringValue(this IEnumerable<IMetadata> metadata, string metadataName, string defaultValue = default, Func<object, string> convertFunction = null)
            => GetMetadataValue(metadata, metadataName, defaultValue, convertFunction);

        /// <summary>
        /// Gets the metadata value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata">The metadata.</param>
        /// <param name="metadataName">Name of the metadata.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="convertFunction">The convert function.</param>
        public static T GetMetadataValue<T>(this IEnumerable<IMetadata> metadata, string metadataName, T defaultValue = default, Func<object, T> convertFunction = null)
        {
            if (metadata == null)
            {
                return defaultValue;
            }
            var metadataItem = metadata.FirstOrDefault(md => md.Name == metadataName);
            if (metadataItem == null)
            {
                return defaultValue;
            }
            if (convertFunction == null)
            {
                if (typeof(T).IsEnum)
                {
                    try
                    {
                        var val = metadataItem.Value?.ToString() ?? string.Empty;
                        return string.IsNullOrEmpty(val)
                            ? defaultValue
                            : (T)Enum.Parse(typeof(T), val, true);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
                if (typeof(T).FullName.StartsWith("System.Nullable`1[[") && typeof(T).GetGenericArguments()[0].IsEnum)
                {
                    try
                    {
                        var val = metadataItem.Value?.ToString() ?? string.Empty;
                        return string.IsNullOrEmpty(val)
                            ? defaultValue
                            : (T)Enum.Parse(typeof(T).GetGenericArguments()[0], val, true);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
                if (metadataItem.Value is T t)
                {
                    return t;
                }
                if (metadataItem.Value != null && typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(metadataItem.Value.ToString(), typeof(T));
                }
                return (T)Convert.ChangeType(metadataItem.Value, typeof(T));
            }
            return convertFunction(metadataItem.Value);
        }

        /// <summary>
        /// Gets the metadata values.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="metadataName">Name of the metadata.</param>
        /// <param name="convertFunction">The convert function.</param>
        public static IEnumerable<string> GetMetadataStringValues(this IEnumerable<IMetadata> metadata, string metadataName, Func<object, string> convertFunction = null)
            => GetMetadataValues(metadata, metadataName, convertFunction);

        /// <summary>
        /// Gets the metadata values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata">The metadata.</param>
        /// <param name="metadataName">Name of the metadata.</param>
        /// <param name="convertFunction">The convert function.</param>
        public static IEnumerable<T> GetMetadataValues<T>(this IEnumerable<IMetadata> metadata, string metadataName, Func<object, T> convertFunction = null)
        {
            if (metadata == null)
            {
                return Array.Empty<T>();
            }
            var result = new List<T>();
            foreach (var metadataItem in metadata.Where(md => md.Name == metadataName))
            {
                if (convertFunction == null)
                {
                    if (typeof(T).IsEnum)
                    {
                        try
                        {
                            var val = metadataItem.Value?.ToString() ?? string.Empty;
                            result.Add(string.IsNullOrEmpty(val)
                                ? default
                                : (T)Enum.Parse(typeof(T), val, true));
                        }
                        catch
                        {
                            result.Add(default);
                        }
                    }
                    if (typeof(T).FullName.StartsWith("System.Nullable`1[[") && typeof(T).GetGenericArguments()[0].IsEnum)
                    {
                        try
                        {
                            var val = metadataItem.Value?.ToString() ?? string.Empty;
                            result.Add(string.IsNullOrEmpty(val)
                                ? default
                                : (T)Enum.Parse(typeof(T).GetGenericArguments()[0], val, true));
                        }
                        catch
                        {
                            result.Add(default);
                        }
                    }
                    if (metadataItem.Value is T t)
                    {
                        result.Add(t);
                    }
                }
                else
                {
                    result.Add(convertFunction(metadataItem.Value));
                }
            }
            return result;
        }
    }
}
