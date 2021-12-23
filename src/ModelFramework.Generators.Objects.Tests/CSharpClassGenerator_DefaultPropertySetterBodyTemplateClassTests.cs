using System.Diagnostics.CodeAnalysis;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements.Builders;
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
            var model = new ClassPropertyBuilder().WithName("Name").WithTypeName("string").Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertySetterBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"set;
");
        }

        [Fact]
        public void GeneratesCodeBodyWhenSetterBodyIsFilled()
        {
            // Arrange
            var model = new ClassPropertyBuilder()
                .WithName("Name")
                .WithTypeName("string")
                .AddSetterCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
                .Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertySetterBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"set
            {
                throw new NotImplementedException();
            }
");
        }
    }
}
