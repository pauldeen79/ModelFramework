using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassGenerator_DefaultBodyTemplateClassTests
    {
        [Fact]
        public void GeneratesOutputForCodeBodyInClassMethod()
        {
            // Arrange
            var model = new ClassMethod("unused", "unused", body: "throw new NotImplementedException();");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"            throw new NotImplementedException();
");
        }

        [Fact]
        public void GeneratesOutputForCodeBodyInClassConstructor()
        {
            // Arrange
            var model = new ClassConstructor(body: "throw new NotImplementedException();");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"            throw new NotImplementedException();
");
        }
    }
}
