using System;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Extensions;

namespace ModelFramework.Objects.Builders
{
    public partial class ClassPropertyBuilder
    {
        public ClassPropertyBuilder ConvertCollectionToEnumerable()
            => AddMetadata
            (
                new MetadataBuilder().WithName(MetadataNames.CustomImmutableArgumentType).WithValue("System.Collections.Generic.IEnumerable<{1}>"),
                new MetadataBuilder().WithName(MetadataNames.CustomImmutableDefaultValue).WithValue("new System.Collections.Generic.List<{1}>({0} ?? new Enumerable.Empty<{1}>())")
            );

        public ClassPropertyBuilder ConvertSinglePropertyToBuilder(string? argumentType = null,
                                                                   string? customBuilderConstructorInitializeExpression = null)
            => AddMetadata
            (
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue(argumentType ?? "{0}Builder"),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderMethodParameterExpression).WithValue("{0}.Build()"),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderConstructorInitializeExpression).WithValue(customBuilderConstructorInitializeExpression ?? (argumentType == null ? "{0} = new {2}Builder(source.{0});" : "{0} = new " + argumentType + "(source.{0});"))
            );

        public ClassPropertyBuilder ConvertCollectionPropertyToBuilder(string? argumentType = null,
                                                                       string? customBuilderConstructorInitializeExpression = null)
            => ConvertCollectionToEnumerable().AddMetadata
            (
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue(argumentType ?? "System.Collections.Generic.IEnumerable<{1}Builder>"),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderMethodParameterExpression).WithValue("{0}.Select(x => x.Build())"),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderConstructorInitializeExpression).WithValue(customBuilderConstructorInitializeExpression ?? (argumentType == null ? "{4}{0}.AddRange(source.{0}.Select(x => new {3}Builder(x)));" : "{4}{0}.AddRange(source.{0}.Select(x => new " + argumentType.GetGenericArguments() + "(x)));"))
            );

        public ClassPropertyBuilder AddBuilderOverload(string methodNameTemplate,
                                                       Type parameterType,
                                                       string parameterNameTemplate,
                                                       string initializeExpression)
            => AddBuilderOverload(methodNameTemplate,
                                  parameterType?.FullName ?? throw new ArgumentException("Type does not have a full name"),
                                  parameterNameTemplate,
                                  initializeExpression);

        public ClassPropertyBuilder AddBuilderOverload(string methodNameTemplate,
                                                       string parameterTypeName,
                                                       string parameterNameTemplate,
                                                       string initializeExpression)
            => AddMetadata
            (
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithOverloadMethodName).WithValue(methodNameTemplate),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithOverloadArgumentType).WithValue(parameterTypeName),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithOverloadArgumentName).WithValue(parameterNameTemplate),
                new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithOverloadInitializeExpression).WithValue(initializeExpression)
            );

        public ClassPropertyBuilder SetDefaultArgumentValueForWithMethod(object defaultValue)
            => AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithDefaultPropertyValue).WithValue(defaultValue));

        public ClassPropertyBuilder SetDefaultValueForBuilderClassConstructor(object defaultValue)
            => AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomImmutableDefaultValue).WithValue(defaultValue));

        public ClassPropertyBuilder SetBuilderWithExpression( string expression)
            => AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithExpression).WithValue(expression));

        public ClassPropertyBuilder AddGetterLiteralCodeStatements(params string[] statements)
            => AddGetterCodeStatements(statements.ToLiteralCodeStatementBuilders());

        public ClassPropertyBuilder AddSetterLiteralCodeStatements(params string[] statements)
            => AddSetterCodeStatements(statements.ToLiteralCodeStatementBuilders());

        public ClassPropertyBuilder AddInitializerLiteralCodeStatements(params string[] statements)
            => AddInitializerCodeStatements(statements.ToLiteralCodeStatementBuilders());
    }
}
