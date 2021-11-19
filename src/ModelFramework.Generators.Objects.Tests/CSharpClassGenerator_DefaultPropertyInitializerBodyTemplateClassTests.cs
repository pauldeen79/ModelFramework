using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassGenerator_DefaultPropertyInitializerBodyTemplateClassTests
    {
        [Fact]
        public void GeneratesNoCodeBodyWhenInitializerBodyIsEmpty()
        {
            // Arrange
            var model = new ClassProperty("Name", "string");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyInitializerBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"init;
");
        }

        [Fact]
        public void GeneratesCodeBodyWhenInitializerBodyIsFilled()
        {
            // Arrange
            var model = new ClassProperty("Name", "string", initializerCodeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyInitializerBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"init
            {
                throw new NotImplementedException();
            }
");
        }
    }
}
