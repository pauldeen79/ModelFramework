using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassGenerator_DefaultEnumMemberTemplateTests
    {
        [Fact]
        public void GeneratesCodeBodyWithoutComma()
        {
            // Arrange
            var model = new EnumMember("Member");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumMemberTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be("            Member");
        }

        [Fact]
        public void GeneratesCodeBodyWithValueWhenPresent()
        {
            // Arrange
            var model = new EnumMember("Member", 1);
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumMemberTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be("            Member = 1");
        }
    }
}
