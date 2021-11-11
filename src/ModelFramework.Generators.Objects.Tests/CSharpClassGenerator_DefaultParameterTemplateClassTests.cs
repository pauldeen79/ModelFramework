using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class Parameter_DefaultClassTests
    {
        [Fact]
        public void GeneratesOutputWithoutComma()
        {
            // Arrange
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new Parameter("Name", "string");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultParameterTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be("string Name");
        }

        [Fact]
        public void GeneratesAttributes()
        {
            // Arrange
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new Parameter("Name", "string", attributes: new[] { new Attribute("Attribute1"), new Attribute("Attribute2"), new Attribute("Attribute3") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultParameterTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be("[Attribute1][Attribute2][Attribute3]string Name");
        }
    }
}
