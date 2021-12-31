using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.Common;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;
using ModelFramework.Common.Extensions;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using ModelFramework.Database.SqlStatements;
using ModelFramework.Generators.Objects;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.CodeGeneration.Tests
{
    [ExcludeFromCodeCoverage]
    public class IntegrationTests
    {
        [Fact]
        public void CanGenerateImmutableBuilderClassesForCommonModelEntities()
        {
            // Arrange
            var models = typeof(Metadata).Assembly.GetExportedTypes()
                .Where(t => t.FullName != null && t.FullName.GetNamespaceWithDefault() == typeof(Metadata).FullName?.GetNamespaceWithDefault())
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(GetNamespace(t)))
                .ToArray();
            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);

            // Act
            var builderModels = models.Select(c => c.Build()
                                                    .ToImmutableBuilderClassBuilder(settings)
                                                    .WithNamespace("ModelFramework.Common.Builders")
                                                    .WithPartial()
                                                    .AddExcludeFromCodeCoverageAttribute()
                                                    .Build()).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForCsharpModelEntities()
        {
            // Arrange
            var models = typeof(Class).Assembly.GetExportedTypes()
                .Where(t => t.FullName != null &&  t.FullName.GetNamespaceWithDefault() == typeof(Class).FullName?.GetNamespaceWithDefault())
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(GetNamespace(t)))
                .ToArray();

            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);

            // Act
            var builderModels = models.Select(c => c.Build()
                                                    .ToImmutableBuilderClassBuilder(settings)
                                                    .WithNamespace("ModelFramework.Objects.Builders")
                                                    .WithPartial()
                                                    .AddExcludeFromCodeCoverageAttribute()
                                                    .AddMethods(CreateAddMetadataOverload(c))
                                                    .Build()).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels, additionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true });

            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForCsharpCodeStatements()
        {
            // Arrange
            var models = typeof(LiteralCodeStatement).Assembly.GetExportedTypes()
                .Where(t => t.FullName != null && t.FullName.GetNamespaceWithDefault() == typeof(LiteralCodeStatement).FullName?.GetNamespaceWithDefault())
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(GetNamespace(t)))
                .ToArray();

            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);

            // Act
            var builderModels = models.Select(c => c.Build()
                                                    .ToImmutableBuilderClassBuilder(settings)
                                                    .WithNamespace("ModelFramework.Objects.CodeStatements.Builders")
                                                    .WithPartial()
                                                    .AddExcludeFromCodeCoverageAttribute()
                                                    .AddMethods(CreateAddMetadataOverload(c))
                                                    .AddInterfaces(typeof(ICodeStatementBuilder))
                                                    .Chain(x => x.Methods.First(x => x.Name == "Build").WithType(typeof(ICodeStatement)))
                                                    .Build()).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels, additionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true });

            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForDatabaseModelEntities()
        {
            // Arrange
            var models = typeof(Table).Assembly.GetExportedTypes()
                .Where(t => t.FullName != null && t.FullName.GetNamespaceWithDefault() == typeof(Table).FullName?.GetNamespaceWithDefault())
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(GetNamespace(t)))
                .ToArray();

            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);

            // Act
            var builderModels = models.Select(c => c.Build()
                                                    .ToImmutableBuilderClassBuilder(settings)
                                                    .WithNamespace("ModelFramework.Database.Builders")
                                                    .WithPartial()
                                                    .AddExcludeFromCodeCoverageAttribute()
                                                    .AddMethods(CreateAddMetadataOverload(c))
                                                    .Build()).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels, additionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true });

            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForDatabaseCodeStalootements()
        {
            // Arrange
            var models = typeof(LiteralSqlStatement).Assembly.GetExportedTypes()
                .Where(t => t.FullName != null && t.FullName.GetNamespaceWithDefault() == typeof(LiteralSqlStatement).FullName?.GetNamespaceWithDefault())
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(GetNamespace(t)))
                .ToArray();

            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);

            // Act
            var builderModels = models.Select(c => c.Build()
                                                    .ToImmutableBuilderClassBuilder(settings)
                                                    .WithNamespace("ModelFramework.Database.SqlStatements.Builders")
                                                    .WithPartial()
                                                    .AddExcludeFromCodeCoverageAttribute()
                                                    .AddMethods(CreateAddMetadataOverload(c))
                                                    .AddInterfaces(typeof(ISqlStatementBuilder))
                                                    .Chain(x => x.Methods.First(x => x.Name == "Build").WithType(typeof(ISqlStatement)))
                                                    .Build()).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels, additionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true });

            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateBuildersForCommonContracts()
        {
            // Arrange
            var settings = new ImmutableBuilderClassSettings(newCollectionTypeName: "CrossCutting.Common.ValueCollection");
            var model = new[]
            {
                typeof(IMetadata)
            }.Select(x => x.ToClassBuilder(new ClassSettings())
                           .Chain(x => x.Attributes.Clear())
                           .WithName(x.Name.Substring(1))
                           .Build()
                           .ToImmutableBuilderClass(settings)
                    ).Cast<ITypeBase>().ToArray();

            var generator = new CSharpClassGenerator();
            generator.Session = new Dictionary<string, object>
            {
                { "Model", model }
            };
            var builder = new StringBuilder();

            // Act
            generator.Initialize();
            generator.Render(builder);
            var output = builder.ToString();

            // Assert
            output.Should().NotBeEmpty();
        }

        private static void FixImmutableBuilderProperties(ClassBuilder[] models)
        {
            foreach (var classBuilder in models)
            {
                foreach (var property in classBuilder.Properties)
                {
                    var typeName = property.TypeName.FixTypeName();
                    if (typeName.StartsWithAny(StringComparison.InvariantCulture, "ModelFramework.Objects.Contracts.I", "ModelFramework.Database.Contracts.I", "ModelFramework.Common.Contracts.I"))
                    {
                        property.ConvertSinglePropertyToBuilder
                        (
                            typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture) + "Builder"
                        );
                    }
                    else if (typeName.Contains("Collection<ModelFramework."))
                    {
                        var isCodeStatement = typeName.Contains("ModelFramework.Objects.Contracts.ICodeStatement") || typeName.Contains("ModelFramework.Database.Contracts.ISqlStatement");
                        property.ConvertCollectionPropertyToBuilder
                        (
                            isCodeStatement
                                ? typeName.ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                                : typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture),
                            isCodeStatement
                                ? "{4}{0}.AddRange(source.{0}.Select(x => x.CreateBuilder()));"
                                : null
                        );
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
        }

        private static string GetNamespace(Type t)
            => t.FullName?.GetNamespaceWithDefault() ?? string.Empty;

        private static string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
        {
            if (instance.Namespace == "ModelFramework.Common.Default")
            {
                return forCreate
                    ? "ModelFramework.Common.Default." + instance.Name
                    : "ModelFramework.Common.Contracts.I" + instance.Name;
            }

            if (instance.Namespace == "ModelFramework.Objects.Default")
            {
                return forCreate
                    ? "ModelFramework.Objects.Default." + instance.Name
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

        private static ClassMethodBuilder CreateAddMetadataOverload(ClassBuilder c)
            => new ClassMethodBuilder()
                .WithName("AddMetadata")
                .WithTypeName($"{c.Name}Builder")
                .AddParameter("name", typeof(string))
                .AddParameters(new ParameterBuilder().WithName("value").WithType(typeof(object)).WithIsNullable())
                .AddLiteralCodeStatements($"AddMetadata(new {typeof(MetadataBuilder).FullName}().WithName(name).WithValue(value));",
                                          "return this;");
    }
}
