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
    public class CSharpClassGenerator_DefaultPropertyInitializerBodyTemplateClassTests
    {
        [Fact]
        public void GeneratesNoCodeBodyWhenInitializerBodyIsEmpty()
        {
            // Arrange
            var model = new ClassPropertyBuilder().WithName("Name").WithTypeName("string").Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyInitializerBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"init;
");
        }

        [Fact]
        public void GeneratesCodeBodyWhenInitializerBodyIsFilled()
        {
            // Arrange
            var model = new ClassPropertyBuilder()
                .WithName("Name")
                .WithTypeName("string")
                .AddInitializerCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
                .Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyInitializerBodyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"init
            {
                throw new NotImplementedException();
            }
");
        }
    }
}
