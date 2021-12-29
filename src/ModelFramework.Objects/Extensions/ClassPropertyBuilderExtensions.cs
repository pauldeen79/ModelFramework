using ModelFramework.Common.Builders;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Builders;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassPropertyBuilderExtensions
    {
        public static ClassPropertyBuilder ConvertCollectionToEnumerable(this ClassPropertyBuilder instance)
            => instance.AddMetadata
            (
                new MetadataBuilder().WithName(MetadataNames.CustomImmutableArgumentType).WithValue("System.Collections.Generic.IEnumerable<{1}>"),
                new MetadataBuilder().WithName(MetadataNames.CustomImmutableDefaultValue).WithValue("new System.Collections.Generic.List<{1}>({0} ?? new Enumerable.Empty<{1}>())")
            );

        public static ClassPropertyBuilder ConvertSinglePropertyToBuilder(this ClassPropertyBuilder instance,
                                                                          string? argumentType = null,
                                                                          string? customBuilderConstructorInitializeExpression = null)
            => instance.AddMetadata
            (
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue(argumentType ?? "{0}Builder"),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderMethodParameterExpression).WithValue("{0}.Build()"),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderConstructorInitializeExpression).WithValue(customBuilderConstructorInitializeExpression ?? (argumentType == null ? "{0} = new {2}Builder(source.{0});" : "{0} = new " + argumentType + "(source.{0});"))
            );

        public static ClassPropertyBuilder ConvertCollectionPropertyToBuilder(this ClassPropertyBuilder instance,
                                                                              string? argumentType = null,
                                                                              string? customBuilderConstructorInitializeExpression = null)
            => instance.ConvertCollectionToEnumerable().AddMetadata
            (
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue(argumentType ?? "System.Collections.Generic.IEnumerable<{1}Builder>"),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderMethodParameterExpression).WithValue("{0}.Select(x => x.Build())"),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderConstructorInitializeExpression).WithValue(customBuilderConstructorInitializeExpression ?? (argumentType == null ? "{4}{0}.AddRange(source.{0}.Select(x => new {3}Builder(x)));" : "{4}{0}.AddRange(source.{0}.Select(x => new " + argumentType.GetGenericArguments() + "(x)));"))
            );

        public static ClassPropertyBuilder AddBuilderOverload(this ClassPropertyBuilder instance,
                                                              string methodNameTemplate,
                                                              string parameterType,
                                                              string parameterNameTemplate,
                                                              string initializeExpression)
            => instance.AddMetadata
            (
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithOverloadMethodName).WithValue(methodNameTemplate),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithOverloadArgumentType).WithValue(parameterType),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithOverloadArgumentName).WithValue(parameterNameTemplate),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithOverloadInitializeExpression).WithValue(initializeExpression)
            );

        public static ClassPropertyBuilder SetDefaultArgumentValueForWithMethod(this ClassPropertyBuilder instance,
                                                                                object defaultValue)
            => instance.AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithDefaultPropertyValue).WithValue(defaultValue));

        public static ClassPropertyBuilder SetDefaultValueForBuilderClassConstructor(this ClassPropertyBuilder instance,
                                                                                     object defaultValue)
            => instance.AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomImmutableDefaultValue).WithValue(defaultValue));

        public static ClassPropertyBuilder SetBuilderWithExpression(this ClassPropertyBuilder instance, string expression)
            => instance.AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithExpression).WithValue(expression));
    }
}
