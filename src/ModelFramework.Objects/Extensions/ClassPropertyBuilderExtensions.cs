using ModelFramework.Common.Default;
using ModelFramework.Objects.Builders;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassPropertyBuilderExtensions
    {
        public static ClassPropertyBuilder ConvertCollectionToEnumerable(this ClassPropertyBuilder instance)
            => instance.AddMetadata
            (
                new Metadata(MetadataNames.CustomImmutableArgumentType, "System.Collections.Generic.IEnumerable<{1}>"),
                new Metadata(MetadataNames.CustomImmutableDefaultValue, "new System.Collections.Generic.List<{1}>({0} ?? new Enumerable.Empty<{1}>())")
            );

        public static ClassPropertyBuilder ConvertSinglePropertyToBuilder(this ClassPropertyBuilder instance)
            => instance.AddMetadata
            (
                new Metadata(MetadataNames.CustomImmutableBuilderArgumentType, "{0}Builder"),
                new Metadata(MetadataNames.CustomImmutableBuilderMethodParameterExpression, "{0}.Build()"),
                new Metadata(MetadataNames.CustomImmutableBuilderConstructorInitializeExpression, "{0} = new {2}Builder(source.{0});"),
                new Metadata(MetadataNames.CustomImmutableBuilderWithOverloadArgumentType, "{0}"),
                new Metadata(MetadataNames.CustomImmutableBuilderWithOverloadInitializeExpression, "{0} = new {1}Builder({0});")
            );

        public static ClassPropertyBuilder ConvertCollectionPropertyToBuilder(this ClassPropertyBuilder instance)
            => instance.ConvertCollectionToEnumerable().AddMetadata
            (
                new Metadata(MetadataNames.CustomImmutableBuilderArgumentType, "System.Collections.Generic.IEnumerable<{1}Builder>"),
                new Metadata(MetadataNames.CustomImmutableBuilderMethodParameterExpression, "{0}.Select(x => x.Build())"),
                new Metadata(MetadataNames.CustomImmutableBuilderConstructorInitializeExpression, "if (source.{0} != null) {0}.AddRange(source.{0}.Select(x => new {3}Builder(x)));"),
                new Metadata(MetadataNames.CustomImmutableBuilderWithOverloadArgumentType, "{0}"),
                new Metadata(MetadataNames.CustomImmutableBuilderWithOverloadInitializeExpression, "        {0}.Add(new {2}Builder(itemToAdd));")
            );
    }
}
