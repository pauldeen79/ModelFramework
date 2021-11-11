using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassPropertyExtensions
    {
        public static string CreateImmutableBuilderInitializationCode(this IClassProperty property)
        {
            var defaultValue = property.TypeName.IsCollectionTypeName()
                ? $"    foreach (var x in source.{property.Name}) _{property.Name.ToPascalCase()}.Add(x);"
                : $"    _{property.Name.ToPascalCase()} = source.{property.Name};";

            return property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderConstructorInitializeExpression, defaultValue, o => string.Format(o?.ToString() ?? string.Empty, property.Name, property.Name.ToPascalCase(), property.TypeName.FixTypeName().GetCsharpFriendlyTypeName(), property.TypeName.GetGenericArguments().GetCsharpFriendlyTypeName()));
        }

        public static string CreateImmutableBuilderClearCode(this IClassProperty property)
            => property.TypeName.IsCollectionTypeName()
                ? $"_{property.Name.ToPascalCase()}.Clear();"
                : $"_{property.Name.ToPascalCase()} = default;";

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

        public static string FixObservablePropertyGetterBody(this IClassProperty property, string newCollectionTypeName)
            => property.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).StartsWith("System.Collections.ObjectModel.ObservableCollection<")
                ? string.Format("return _{0};", property.Name.ToPascalCase())
                : property.GetterBody;

        public static string FixObservablePropertySetterBody(this IClassProperty property, string newCollectionTypeName)
            => property.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).StartsWith("System.Collections.ObjectModel.ObservableCollection<")
                ? string.Format("_{0} = value;", property.Name.ToPascalCase()) + Environment.NewLine + string.Format(@"if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(""{0}""));", property.Name)
                : property.SetterBody;
    }
}
