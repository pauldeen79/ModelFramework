using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassGenerator_DefaultPropertySetterBodyTemplateClassTests
    {
        [Fact]
        public void GeneratesNoCodeBodyWhenSetterBodyIsEmpty()
        {
            // Arrange
            var model = new ClassProperty("Name", "string");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertySetterBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"set;
");
        }

        [Fact]
        public void GeneratesCodeBodyWhenSetterBodyIsFilled()
        {
            // Arrange
            var model = new ClassProperty("Name", "string", setterBody: "throw new NotImplementedException();");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertySetterBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"set
            {
                throw new NotImplementedException();
            }
");
        }
    }
}
