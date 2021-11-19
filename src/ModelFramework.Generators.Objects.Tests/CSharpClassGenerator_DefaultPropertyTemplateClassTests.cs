using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassGenerator_DefaultPropertyTemplateClassTests
    {
        [Fact]
        public void GeneratesCodeBodyFromCodeStatementsCorrectly()
        {
            // Arrange
            var model = new ClassProperty("Name", "string"
                , getterCodeStatements: new ICodeStatement[] { new LiteralCodeStatement("return null;") }
                , setterCodeStatements: new ICodeStatement[] { new LiteralCodeStatement("// ignore value") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public string Name
        {
            get
            {
                return null;
            }
            set
            {
                // ignore value
            }
        }
");
        }

        [Fact]
        public void Generates_Property_Correctly()
        {
            // Arrange
            var model = new ClassPropertyBuilder()
                .WithName("MyProperty")
                .WithTypeName(typeof(string).FullName)
                .Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public string MyProperty
        {
            get;
            set;
        }
");
        }

        [Fact]
        public void Generates_Internal_Property_Correctly()
        {
            // Arrange
            var model = new ClassPropertyBuilder()
                .WithName("MyProperty")
                .WithTypeName(typeof(string).FullName)
                .WithVisibility(Visibility.Internal)
                .Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        internal string MyProperty
        {
            get;
            set;
        }
");
        }

        [Fact]
        public void Generates_Init_Property_Correctly()
        {
            // Arrange
            var model = new ClassPropertyBuilder()
                .WithName("MyProperty")
                .WithTypeName(typeof(string).FullName)
                .WithHasInitializer()
                .Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public string MyProperty
        {
            get;
            init;
        }
");
        }
    }
}
