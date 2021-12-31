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
using ModelFramework.Generators.Objects;
using ModelFramework.Objects.Builders;
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
                .Where(t => t.FullName?.StartsWith("ModelFramework.Common.Default.") == true)
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
                .Where(t => t.FullName?.StartsWith("ModelFramework.Objects.Default.") == true)
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
            var models = typeof(Class).Assembly.GetExportedTypes()
                .Where(t => t.FullName?.StartsWith("ModelFramework.Objects.CodeStatements.") == true && t.FullName?.Contains(".Builders") != true)
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
                .Where(t => t.FullName?.StartsWith("ModelFramework.Database.Default.") == true)
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
        public void CanGenerateImmutableBuilderClassesForDatabaseCodeStatements()
        {
            // Arrange
            var models = typeof(Table).Assembly.GetExportedTypes()
                .Where(t => t.FullName?.StartsWith("ModelFramework.Database.SqlStatements.") == true && t.FullName?.Contains(".Builders") != true)
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
                    else if (typeName == typeof(bool).FullName || typeName == typeof(bool?).FullName)
                    {
                        property.SetDefaultArgumentValueForWithMethod(true);
                        if (property.Name == "HasGetter" || property.Name == "HasSetter")
                        {
                            property.SetDefaultValueForBuilderClassConstructor(new Literal("true"));
                        }
                    }
                    else if (property.Name == "TypeName" && property.TypeName == typeof(string).FullName)
                    {
                        property.AddBuilderOverload("WithType", typeof(Type), "type", "{2} = type.FullName;");
                    }

                    if (property.Name == "Visibility")
                    {
                        property.SetDefaultValueForBuilderClassConstructor
                        (
                            new Literal
                            (
                                classBuilder.Name == "ClassField"
                                    ? "ModelFramework.Objects.Contracts.Visibility.Private"
                                    : "ModelFramework.Objects.Contracts.Visibility.Public"
                            )
                        );
                    }

                    if (property.Name == "HasSetter")
                    {
                        property.SetBuilderWithExpression(@"{0} = {2};
if ({2})
{5}
    HasInitializer = false;
{6}");
                    }

                    if (property.Name == "HasInitializer")
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
            => t.FullName?.GetNamespaceWithDefault(string.Empty) ?? string.Empty;

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
