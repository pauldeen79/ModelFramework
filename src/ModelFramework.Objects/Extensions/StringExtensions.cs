using System;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceGeneric(this string typeName, string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (typeName.StartsWith(prefix + "<") && typeName.EndsWith(">"))
            {
                return typeName.Substring(prefix.Length + 1, typeName.Length - prefix.Length - 2);
            }
            return typeName;
        }

        public static string FixImmutableCollectionTypeName(this string typeName, string newCollectionTypeName)
            => typeName
                .FixTypeName()
                .Replace("System.Collections.Generic.IEnumerable<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.ICollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.IReadOnlyCollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.IList<", newCollectionTypeName + "<")
                .Replace("System.Collections.ObjectModel.Collection<", newCollectionTypeName + "<")
                .Replace("System.Collections.ObjectModel.ObservableCollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.ObjectModel.ReadOnlyCollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.List<", newCollectionTypeName + "<")
                .Replace("CrossCutting.Common.ValueCollection<", newCollectionTypeName + "<");

        public static string FixBuilderCollectionTypeName(this string typeName, string newCollectionTypeName)
            => typeName
                .FixTypeName()
                .Replace("System.Collections.Generic.IEnumerable<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.IList<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.List<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.ICollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.IReadOnlyCollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.ObjectModel.Collection<", newCollectionTypeName + "<")
                .Replace("System.Collections.ObjectModel.ReadOnlyCollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.ObjectModel.ObservableCollection<", newCollectionTypeName + "<")
                .Replace("CrossCutting.Common.ValueCollection<", newCollectionTypeName + "<");

        public static string ConvertTypeNameToArray(this string typeName)
            => typeName
                .FixTypeName()
                .ReplaceGeneric("System.Collections.Generic.IEnumerable")
                .ReplaceGeneric("System.Collections.Generic.IList")
                .ReplaceGeneric("System.Collections.Generic.List")
                .ReplaceGeneric("System.Collections.Generic.ICollection")
                .ReplaceGeneric("System.Collections.ObjectModel.Collection")
                .ReplaceGeneric("System.Collections.ObjectModel.ObservableCollection")
                .ReplaceGeneric("CrossCutting.Common.ValueCollection")
                + "[]";

        public static string FixObservableCollectionTypeName(this string typeName, string newCollectionTypeName)
            => typeName
                .FixTypeName()
                .Replace("System.Collections.Generic.IEnumerable<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.IList<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.List<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.ICollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.Generic.IReadOnlyCollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.ObjectModel.ReadOnlyCollection<", newCollectionTypeName + "<")
                .Replace("System.Collections.ObjectModel.Collection<", newCollectionTypeName + "<")
                .Replace("CrossCutting.Common.ValueCollection<", newCollectionTypeName + "<");

        public static bool IsCollectionTypeName(this string typeName)
            => typeName.FixTypeName().StartsWithAny
                (
                    "System.Collections.Generic.IEnumerable<",
                    "System.Collections.Generic.IList<",
                    "System.Collections.Generic.List<",
                    "System.Collections.Generic.ICollection<",
                    "System.Collections.Generic.IReadOnlyCollection<",
                    "System.Collections.ObjectModel.Collection<",
                    "System.Collections.ObjectModel.ReadOnlyCollection<",
                    "System.Collections.ObjectModel.ObservableCollection<",
                    "CrossCutting.Common.ValueCollection<"
                );

        public static string RemoveInterfacePrefix(this string name)
        {
            if (name == null)
            {
                return null;
            }
            var index = name.IndexOf(".");
            if (index == -1)
            {
                return name;
            }
            return name.Substring(index + 1);
        }

        public static string AppendNullableAnnotation(this string instance,
                                                      ITypeContainer typeContainer,
                                                      bool enableNullableReferenceTypes)
            => !string.IsNullOrEmpty(instance)
                && !instance.StartsWith("System.Nullable")
                && !instance.EndsWith("?")
                && typeContainer.IsNullable
                && enableNullableReferenceTypes
                    ? $"{instance}?"
                    : instance;
    }
}
