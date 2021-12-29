using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.Common;
using ModelFramework.Common.Default;
using ModelFramework.Common.Extensions;
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
        public void GeneratesImmutableBuilderClassesForAllCommonModelEntities()
        {
            // Arrange
            var models = typeof(Metadata).Assembly.GetExportedTypes()
                .Where(t => t.FullName.StartsWith("ModelFramework.Common.Default."))
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(FixNamespace(t)))
                .ToArray();
            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(newCollectionTypeName: "System.Collections.Generic.IReadOnlyCollection",
                                                             constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);

            // Act
            var builderModels = models.SelectMany(c => new[] { c.Build().ToImmutableBuilderClassBuilder(settings).WithNamespace("ModelFramework.Common.Builders").Build() }).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels);

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForAllCsharpModelEntities()
        {
            // Arrange
            var models = typeof(Class).Assembly.GetExportedTypes()
                .Where(t => t.FullName.StartsWith("ModelFramework.Objects.Default."))
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(FixNamespace(t)))
                .ToArray();

            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);
            // Act
            var builderModels = models.SelectMany(c => new[] { c.Build().ToImmutableBuilderClassBuilder(settings).WithNamespace("ModelFramework.Objects.Builders").WithPartial().AddExcludeFromCodeCoverageAttribute().Build() }).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels, additionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true });

            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForAllDatabaseModelEntities()
        {
            // Arrange
            var models = typeof(Table).Assembly.GetExportedTypes()
                .Where(t => t.FullName.StartsWith("ModelFramework.Database.Default."))
                .Select(t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name).WithNamespace(FixNamespace(t)))
                .ToArray();

            FixImmutableBuilderProperties(models);
            var settings = new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true),
                                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);
            // Act
            var builderModels = models.SelectMany(c => new[] { c.Build().ToImmutableBuilderClassBuilder(settings).WithNamespace("ModelFramework.Database.Builders").WithPartial().AddExcludeFromCodeCoverageAttribute().Build() }).ToArray();
            var sut = new CSharpClassGenerator();
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, builderModels, additionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true });

            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
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
                        property.ConvertCollectionPropertyToBuilder
                        (
                            typeName.Contains("ModelFramework.Objects.Contracts.ICodeStatement")
                                ? typeName.ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                                : typeName.Replace("Contracts.I", "Builders.", StringComparison.InvariantCulture).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture),
                            typeName.Contains("ModelFramework.Objects.Contracts.ICodeStatement")
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
                        property.AddBuilderOverload("WithType", typeof(Type).FullName, "type", "{2} = type.FullName;");
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

        private static string FixNamespace(Type t)
        {
            if (t.FullName.StartsWith("ModelFramework.Common.Default"))
            {
                return t.FullName.Replace("ModelFramework.Common.Default", "ModelFramework.Common.Contracts").GetNamespaceWithDefault(string.Empty);
            }
            return t.FullName.GetNamespaceWithDefault(string.Empty);
        }

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
    }
}
