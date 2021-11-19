using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassGenerator_DefaultPropertyGetterBodyTemplateClassTests
    {
        [Fact]
        public void GeneratesNoCodeBodyWhenGetterBodyIsEmpty()
        {
            // Arrange
            var model = new ClassProperty("Name", "string");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyGetterBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"get;
");
        }

        [Fact]
        public void GeneratesCodeBodyWhenGetterBodyIsFilled()
        {
            // Arrange
            var model = new ClassProperty("Name", "string", getterCodeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyGetterBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"get
            {
                throw new NotImplementedException();
            }
");
        }
    }
}
