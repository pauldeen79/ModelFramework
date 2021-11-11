using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassGenerator_DefaultAttributeTemplateClassTests
    {
        [Fact]
        public void GeneratesAttributeCode()
        {
            // Arrange
            var model = new Attribute("Attribute1");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultAttributeTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        [Attribute1]
");
        }

        [Fact]
        public void GeneratesCodeForAttributeWithParameterWithNameAndValue()
        {
            // Arrange
            var model = new Attribute("Attribute1", new[] { new AttributeParameter("Value", "Name") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultAttributeTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        [Attribute1(Name = @""Value"")]
");
        }

        [Fact]
        public void GeneratesCodeForAttributeWithParameterWithValueOnly()
        {
            // Arrange
            var model = new Attribute("Attribute1", new[] { new AttributeParameter("Value") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultAttributeTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        [Attribute1(@""Value"")]
");
        }

        //IndentFourSpacesOnClass
        //IndentEightSpacesOnChildItem
    }
}
