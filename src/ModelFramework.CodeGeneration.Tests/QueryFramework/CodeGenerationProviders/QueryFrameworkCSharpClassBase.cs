using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using ModelFramework.CodeGeneration.CodeGenerationProviders;
using ModelFramework.Common;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;

namespace ModelFramework.CodeGeneration.Tests.QueryFramework.CodeGenerationProviders
{
    public abstract class QueryFrameworkCSharpClassBase : CSharpClassBase
    {
        protected override bool CreateCodeGenerationHeader => true;
        protected override bool EnableNullableContext => true;
        protected override Type RecordCollectionType => typeof(ValueCollection<>);

        protected override IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
            => Enumerable.Empty<ClassMethodBuilder>();

        protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
        {
            if (instance.Namespace == "QueryFramework.Core")
            {
                return forCreate
                    ? "QueryFramework.Core." + instance.Name
                    : "QueryFramework.Abstractions.I" + instance.Name;
            }

            return string.Empty;
        }

        protected override void FixImmutableBuilderProperties(ClassBuilder classBuilder)
        {
            foreach (var property in classBuilder.Properties)
            {
                var typeName = property.TypeName.FixTypeName();
                if (typeName.StartsWith("QueryFramework.Abstractions.I", StringComparison.InvariantCulture))
                {
                    property.ConvertSinglePropertyToBuilderOnBuilder
                    (
                        typeName.Replace("QueryFramework.Abstractions.", "QueryFramework.Abstractions.Builders.") + "Builder",
                        GetCustomBuilderConstructorInitializeExpression(property, typeName)
                    );

                    property.SetDefaultValueForBuilderClassConstructor(new Literal("new " + typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.") + "Builder()"));
                }
                else if (typeName.Contains("Collection<QueryFramework."))
                {
                    property.ConvertCollectionPropertyToBuilderOnBuilder
                    (
                        false,
                        typeof(ValueCollection<>).WithoutGenerics(),
                        typeName.Replace("QueryFramework.Abstractions.", "QueryFramework.Core.Builders.").ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                    );
                }
                else if (typeName.Contains("Collection<System.String"))
                {
                    property.AddMetadata(ModelFramework.Objects.MetadataNames.CustomBuilderMethodParameterExpression, $"new {typeof(ValueCollection<string>).FullName?.FixTypeName()}({{0}})");
                }
                else if (typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName())
                {
                    property.SetDefaultArgumentValueForWithMethod(true);
                    if (property.Name == nameof(ClassProperty.HasGetter) || property.Name == nameof(ClassProperty.HasSetter))
                    {
                        property.SetDefaultValueForBuilderClassConstructor(new Literal("true"));
                    }
                }
            }
        }

        private static string GetCustomBuilderConstructorInitializeExpression(ClassPropertyBuilder property, string typeName)
        {
            if (typeName == "QueryFramework.Abstractions.IQueryExpressionFunction")
            {
                return property.IsNullable
                    ? "{0} = source.{0} == null ? null : source.{0}.ToBuilder()"
                    : "{0} = source.{0}.ToBuilder()";
            }

            return property.IsNullable
                ? "{0} = source.{0} == null ? null : new " + typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.") + "Builder" + "(source.{0})"
                : "{0} = new " + typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.") + "Builder" + "(source.{0})";
        }

        protected static IClass[] GetModels()
            => new[]
            {
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"OpenBracket",
                            TypeName = @"System.Boolean",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"CloseBracket",
                            TypeName = @"System.Boolean",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Field",
                            TypeName = @"QueryFramework.Abstractions.IQueryExpression",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Operator",
                            TypeName = @"QueryFramework.Abstractions.QueryOperator",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Value",
                            TypeName = @"System.Object",
                            IsNullable = true,
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Combination",
                            TypeName = @"QueryFramework.Abstractions.QueryCombination",
                        },
                    } ),
                    Name = @"IQueryCondition",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"FieldName",
                            TypeName = @"System.String",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Function",
                            TypeName = @"QueryFramework.Abstractions.IQueryExpressionFunction",
                            IsNullable = true,
                        },
                    } ),
                    Name = @"IQueryExpression",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"InnerFunction",
                            TypeName = @"QueryFramework.Abstractions.IQueryExpressionFunction",
                            IsNullable = true,
                        },
                    } ),
                    Name = @"IQueryExpressionFunction",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Name",
                            TypeName = @"System.String",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Value",
                            TypeName = @"System.Object",
                        },
                    } ),
                    Name = @"IQueryParameter",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Name",
                            TypeName = @"System.String",
                        },
                    } ),
                    Name = @"IQueryParameterValue",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Field",
                            TypeName = @"QueryFramework.Abstractions.IQueryExpression",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Order",
                            TypeName = @"QueryFramework.Abstractions.QuerySortOrderDirection",
                        },
                    } ),
                    Name = @"IQuerySortOrder",
                },
            }.Select(x => x.Build()).ToArray();
    }
}
