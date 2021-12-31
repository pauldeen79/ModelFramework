using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ModelFramework.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassPropertyExtensions
    {
        public static string CreateImmutableBuilderInitializationCode(this IClassProperty property, bool addNullChecks)
        {
            var defaultValue = property.TypeName.IsCollectionTypeName()
                ? CreateCollectionInitialization(property, addNullChecks)
                : $"{property.Name} = source.{property.Name};";

            return string.Format
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderConstructorInitializeExpression, defaultValue),
                property.Name,
                property.Name.ToPascalCase(),
                property.TypeName.FixTypeName().GetCsharpFriendlyTypeName(),
                property.TypeName.GetGenericArguments().GetCsharpFriendlyTypeName(),
                addNullChecks ? $"if (source.{property.Name} != null) " : ""
            );
        }

        private static string CreateCollectionInitialization(IClassProperty property, bool addNullChecks)
            => addNullChecks
                ? $"if (source.{property.Name} != null) {property.Name}.AddRange(source.{property.Name});"
                : $"{property.Name}.AddRange(source.{property.Name});";

        internal static string GetDefaultValue(this IClassProperty property)
        {
            var md = property.Metadata.FirstOrDefault(x => x.Name == MetadataNames.CustomImmutableDefaultValue);
            if (md != null && md.Value != null)
            {
                if (md.Value is Literal literal && literal.Value != null)
                {
                    return literal.Value;
                }
                return md.Value.CsharpFormat();
            }

            if (property.TypeName.IsStringTypeName() && !property.IsNullable)
            {
                return "string.Empty";
            }

            if (property.TypeName.IsObjectTypeName() && !property.IsNullable)
            {
                return "new object()";
            }

            return "default";
        }

        public static IEnumerable<IMetadata> GetImmutableCollectionMetadata(this IClassProperty property, string newCollectionTypeName)
            => property.TypeName.IsCollectionTypeName()
                ? new[]
                  {
                    new Metadata(MetadataNames.CustomImmutableArgumentType, property.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName).GetCsharpFriendlyTypeName()),
                  }
                : Array.Empty<IMetadata>();

        public static IEnumerable<IMetadata> GetBuilderCollectionMetadata(this IClassProperty property, string newCollectionTypeName)
            => property.TypeName.IsCollectionTypeName()
                ? new[]
                  {
                    new Metadata(MetadataNames.CustomBuilderArgumentType, property.TypeName.FixBuilderCollectionTypeName(newCollectionTypeName))
                  }
                : Array.Empty<IMetadata>();

        public static IEnumerable<IMetadata> GetObservableCollectionMetadata(this IClassProperty property, string newCollectionTypeName)
            => property.TypeName.IsCollectionTypeName()
                ? new[]
                  {
                    new Metadata(MetadataNames.CustomObservableArgumentType, property.TypeName.FixObservableCollectionTypeName(newCollectionTypeName))
                  }
                : Array.Empty<IMetadata>();

        public static IEnumerable<ICodeStatement> FixObservablePropertyGetterBody(this IClassProperty property, string newCollectionTypeName)
            => !property.GetterCodeStatements.Any() && !property.SetterCodeStatements.Any()
                ? new[] { string.Format("return _{0};", property.Name.ToPascalCase()) }.ToLiteralCodeStatements()
                : property.GetterCodeStatements;

        public static IEnumerable<ICodeStatement> FixObservablePropertySetterBody(this IClassProperty property, string newCollectionTypeName)
            => !property.GetterCodeStatements.Any() && !property.SetterCodeStatements.Any()
                ? new[] { string.Format("_{0} = value;", property.Name.ToPascalCase()) + Environment.NewLine + string.Format(@"if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(""{0}""));", property.Name) }.ToLiteralCodeStatements()
                : property.SetterCodeStatements;

        public static string GetGetterModifiers(this IClassProperty property)
            => property.GetSubModifiers(property.GetterVisibility, MetadataNames.PropertyGetterVisibility);

        public static string GetSetterModifiers(this IClassProperty property)
            => property.GetSubModifiers(property.SetterVisibility, MetadataNames.PropertySetterVisibility);

        public static string GetInitModifiers(this IClassProperty property)
            => property.GetSubModifiers(property.InitializerVisibility, MetadataNames.PropertyInitVisibility);

        private static string GetSubModifiers(this IClassProperty property, Visibility? subVisibility, string customModifiersMetadatName)
        {
            var customModifiers = property.Metadata.GetStringValue(customModifiersMetadatName);
            if (!string.IsNullOrEmpty(customModifiers)
                && customModifiers != property.Visibility.ToString().ToLower(CultureInfo.InvariantCulture))
            {
                return customModifiers + " ";
            }
            var builder = new StringBuilder();

            builder.AddWithCondition(subVisibility?.ToString()?.ToLower(CultureInfo.InvariantCulture), subVisibility != null && subVisibility != property.Visibility);

            if (builder.Length > 0)
            {
                builder.Append(" ");
            }

            return builder.ToString();
        }
    }
}
