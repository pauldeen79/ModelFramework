using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.Common;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Extensions;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.SqlStatements;
using ModelFramework.Generators.Objects;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.CodeGeneration.Tests
{
    [ExcludeFromCodeCoverage]
    public class CodeGenerationTests
    {
        [Fact]
        public void CanGenerateImmutableBuilderClassesForCommonContracts()
        {
            // Arrange
            var models = GetImmutableBuilderClasses(GetCommonModelTypes(), "ModelFramework.Common", "ModelFramework.Common.Builders");

            // Act
            var actual = CreateCode(models);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForCsharpContracts()
        {
            // Arrange
            var models = GetImmutableBuilderClasses(GetCsharpModelTypes(), "ModelFramework.Objects", "ModelFramework.Objects.Builders");

            // Act
            var actual = CreateCode(models);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForCsharpCodeStatements()
        {
            // Arrange
            var builderModels = GetClassesFromSameNamespace(typeof(LiteralCodeStatement))
                .Select
                (
                    c => CreateBuilder(c, "ModelFramework.Objects.CodeStatements.Builders")
                        .AddInterfaces(typeof(ICodeStatementBuilder))
                        .Chain(x => x.Methods.First(x => x.Name == nameof(ICodeStatementBuilder.Build)).WithType(typeof(ICodeStatement)))
                        .Build()
                ).ToArray();

            // Act
            var actual = CreateCode(builderModels);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForDatabaseContracts()
        {
            // Arrange
            var models = GetImmutableBuilderClasses(GetDatabaseModelTypes(), "ModelFramework.Database.Default", "ModelFramework.Database.Builders");

            // Act
            var actual = CreateCode(models);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForDatabaseCodeStatements()
        {
            // Arrange
            var builderModels = GetClassesFromSameNamespace(typeof(LiteralSqlStatement))
                .Select
                (
                    c => CreateBuilder(c, "ModelFramework.Database.SqlStatements.Builders")
                        .AddInterfaces(typeof(ISqlStatementBuilder))
                        .Chain(x => x.Methods.First(x => x.Name == nameof(ISqlStatementBuilder.Build)).WithType(typeof(ISqlStatement)))
                        .Build()
                ).ToArray();

            // Act
            var actual = CreateCode(builderModels);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateRecordsForCommonContracts()
        {
            // Arrange
            var models = GetImmutableClasses(GetCommonModelTypes(), "ModelFramework.Common");

            // Act
            var actual = CreateCode(models);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateRecordsForCsharpContracts()
        {
            // Arrange
            var models = GetImmutableClasses(GetCsharpModelTypes(), "ModelFramework.Objects");

            // Act
            var actual = CreateCode(models);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateRecordsForDatabaseContracts()
        {
            // Arrange
            var models = GetImmutableClasses(GetDatabaseModelTypes(), "ModelFramework.Database.Default");

            // Act
            var actual = CreateCode(models);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        private static IClass[] GetClassesFromSameNamespace(Type type)
        {
            if (type.FullName == null)
            {
                throw new ArgumentException("Can't get classes from same namespace when the FullName of this type is null. Could not determine namespace.");
            }

            var models = type.Assembly.GetExportedTypes()
                .Where(t => t.FullName != null
                    && t.FullName.GetNamespaceWithDefault() == type.FullName.GetNamespaceWithDefault()
                    && t.GetProperties().Any())
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name)
                                                                                          .WithNamespace(t.FullName?.GetNamespaceWithDefault() ?? string.Empty))
                .ToArray();

            FixImmutableBuilderProperties(models);

            return models.Select(x => x.Build()).ToArray();
        }

        private static void FixImmutableBuilderProperties(ClassBuilder classBuilder)
        {
            foreach (var property in classBuilder.Properties)
            {
                var typeName = property.TypeName.FixTypeName();
                if (typeName.StartsWithAny(StringComparison.InvariantCulture, "ModelFramework.Objects.Contracts.I",
                                                                              "ModelFramework.Database.Contracts.I",
                                                                              "ModelFramework.Common.Contracts.I"))
                {
                    property.ConvertSinglePropertyToBuilderOnBuilder
                    (
                        typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture) + "Builder"
                    );
                }
                else if (typeName.Contains("Collection<ModelFramework."))
                {
                    var isCodeStatement = typeName.Contains("ModelFramework.Objects.Contracts.ICodeStatement")
                        || typeName.Contains("ModelFramework.Database.Contracts.ISqlStatement");
                    property.ConvertCollectionPropertyToBuilderOnBuilder
                    (
                        false,
                        typeof(ValueCollection<>).WithoutGenerics(),
                        isCodeStatement
                            ? typeName.ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                            : typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture),
                        isCodeStatement
                            ? "{4}{0}.AddRange(source.{0}.Select(x => x.CreateBuilder()));"
                            : null
                    );
                }
                else if (typeName.Contains("Collection<System.String"))
                {
                    property.AddMetadata(Objects.MetadataNames.CustomBuilderMethodParameterExpression, $"new {typeof(ValueCollection<string>).FullName?.FixTypeName()}({{0}})");
                }
                else if (typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName())
                {
                    property.SetDefaultArgumentValueForWithMethod(true);
                    if (property.Name == nameof(ClassProperty.HasGetter) || property.Name == nameof(ClassProperty.HasSetter))
                    {
                        property.SetDefaultValueForBuilderClassConstructor(new Literal("true"));
                    }
                }
                else if (property.Name == nameof(ITypeContainer.TypeName) && property.TypeName.IsStringTypeName())
                {
                    property.AddBuilderOverload("WithType", typeof(Type), "type", "{2} = type.AssemblyQualifiedName;");
                }

                if (property.Name == nameof(IVisibilityContainer.Visibility))
                {
                    property.SetDefaultValueForBuilderClassConstructor
                    (
                        new Literal
                        (
                            classBuilder.Name == nameof(ClassField)
                                ? $"{typeof(Visibility).FullName}.{Visibility.Private}"
                                : $"{typeof(Visibility).FullName}.{Visibility.Public}"
                        )
                    );
                }

                if (property.Name == nameof(ClassProperty.HasSetter))
                {
                    property.SetBuilderWithExpression(@"{0} = {2};
if ({2})
{5}
    HasInitializer = false;
{6}");
                }

                if (property.Name == nameof(ClassProperty.HasInitializer))
                {
                    property.SetBuilderWithExpression(@"{0} = {2};
if ({2})
{5}
    HasSetter = false;
{6}");
                }
            }
        }

        private static void FixImmutableBuilderProperties(ClassBuilder[] models)
        {
            foreach (var classBuilder in models)
            {
                FixImmutableBuilderProperties(classBuilder);
            }
        }

        private static ClassBuilder CreateBuilder(IClass c, string @namespace)
            => c.ToImmutableBuilderClassBuilder(new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                formatInstanceTypeNameDelegate: FormatInstanceTypeName))
                .WithNamespace(@namespace)
                .WithPartial()
                .AddMethods(CreateExtraOverloads(c));

        private static string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
        {
            if (instance.Namespace == "ModelFramework.Common")
            {
                return forCreate
                    ? "ModelFramework.Common." + instance.Name
                    : "ModelFramework.Common.Contracts.I" + instance.Name;
            }

            if (instance.Namespace == "ModelFramework.Objects")
            {
                return forCreate
                    ? "ModelFramework.Objects." + instance.Name
                    : "ModelFramework.Objects.Contracts.I" + instance.Name;
            }

            if (instance.Namespace == "ModelFramework.Database.Default")
            {
                return forCreate
                    ? "ModelFramework.Database.Default." + instance.Name
                    : "ModelFramework.Database.Contracts.I" + instance.Name;
            }

            return string.Empty;
        }

        private static IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
        {
            if (c.Properties.Any(p => p.Name == nameof(IMetadataContainer.Metadata)))
            {
                yield return new ClassMethodBuilder()
                    .WithName("AddMetadata")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameter("name", typeof(string))
                    .AddParameters(new ParameterBuilder().WithName("value").WithType(typeof(object)).WithIsNullable())
                    .AddLiteralCodeStatements($"AddMetadata(new {typeof(MetadataBuilder).FullName}().WithName(name).WithValue(value));",
                                               "return this;");
            }

            if (c.Properties.Any(p => p.Name == nameof(IParametersContainer.Parameters) && p.TypeName.FixTypeName().GetGenericArguments().GetClassName() == nameof(IParameter)))
            {
                yield return new ClassMethodBuilder()
                    .WithName("AddParameter")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameter("name", typeof(string))
                    .AddParameter("type", typeof(Type))
                    .AddLiteralCodeStatements("return AddParameters(new ParameterBuilder().WithName(name).WithType(type));");
                yield return new ClassMethodBuilder()
                    .WithName("AddParameter")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameter("name", typeof(string))
                    .AddParameter("typeName", typeof(string))
                    .AddLiteralCodeStatements("return AddParameters(new ParameterBuilder().WithName(name).WithTypeName(typeName));");
            }

            if (c.Properties.Any(p => p.Name == nameof(ICodeStatementsContainer.CodeStatements)))
            {
                yield return new ClassMethodBuilder()
                    .WithName("AddLiteralCodeStatements")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameters(new ParameterBuilder().WithName("statements").WithType(typeof(string[])).WithIsParamArray())
                    .AddLiteralCodeStatements("return AddCodeStatements(ModelFramework.Objects.Extensions.EnumerableOfStringExtensions.ToLiteralCodeStatementBuilders(statements));");
                yield return new ClassMethodBuilder()
                    .WithName("AddLiteralCodeStatements")
                    .WithTypeName($"{c.Name}Builder")
                    .AddParameters(new ParameterBuilder().WithName("statements").WithType(typeof(IEnumerable<string>)))
                    .AddLiteralCodeStatements("return AddLiteralCodeStatements(statements.ToArray());");
            }
        }

        private static string CreateCode(ITypeBase[] models)
            => TemplateRenderHelper.GetTemplateOutput(new CSharpClassGenerator(),
                                                      models,
                                                      additionalParameters: new
                                                      {
                                                          EnableNullableContext = true,
                                                          CreateCodeGenerationHeader = true
                                                      });

        private static ImmutableClassSettings CreateImmutableClassSettings()
            => new ImmutableClassSettings(newCollectionTypeName: typeof(ValueCollection<>).WithoutGenerics(),
                                          validateArgumentsInConstructor: true);

        private static IClass[] GetImmutableBuilderClasses(Type[] types, string entitiesNamespace, string buildersNamespace)
            => types.Select(x => CreateBuilder(x.ToClassBuilder(new ClassSettings())
                                                .WithName(x.Name.Substring(1))
                                                .WithNamespace(entitiesNamespace)
                                                .Chain(x => FixImmutableBuilderProperties(x))
                                                .Build()
                                                .ToImmutableClass(CreateImmutableClassSettings()), buildersNamespace)
                                                .Build()
                           ).ToArray();

        private static IClass[] GetImmutableClasses(Type[] types, string entitiesNamespace)
            => types.Select(x => x.ToClassBuilder(new ClassSettings())
                                  .WithName(x.Name.Substring(1))
                                  .WithNamespace(entitiesNamespace)
                                  .Chain(x => FixImmutableBuilderProperties(x))
                                  .Build()
                                  .ToImmutableClassBuilder(CreateImmutableClassSettings())
                                  .WithRecord()
                                  .WithPartial()
                                  .AddInterfaces(x)
                                  .Build()
                           ).ToArray();

        private static Type[] GetCommonModelTypes()
            => new[]
            {
                typeof(IMetadata)
            };

        private static Type[] GetCsharpModelTypes()
            => new[]
            {
                typeof(IAttribute),
                typeof(IAttributeParameter),
                typeof(IClass),
                typeof(IClassConstructor),
                typeof(IClassField),
                typeof(IClassMethod),
                typeof(IClassProperty),
                typeof(IEnum),
                typeof(IEnumMember),
                typeof(IInterface),
                typeof(IParameter)
            };

        private static Type[] GetDatabaseModelTypes()
            => new[]
            {
                typeof(ICheckConstraint),
                typeof(IDefaultValueConstraint),
                typeof(IForeignKeyConstraint),
                typeof(IForeignKeyConstraintField),
                typeof(IIndex),
                typeof(IIndexField),
                typeof(IPrimaryKeyConstraint),
                typeof(IPrimaryKeyConstraintField),
                typeof(ISchema),
                typeof(IStoredProcedure),
                typeof(IStoredProcedureParameter),
                typeof(ITable),
                typeof(ITableField),
                typeof(IUniqueConstraint),
                typeof(IUniqueConstraintField),
                typeof(IView),
                typeof(IViewCondition),
                typeof(IViewField),
                typeof(IViewOrderByField),
                typeof(IViewSource)
            };
    }
}
