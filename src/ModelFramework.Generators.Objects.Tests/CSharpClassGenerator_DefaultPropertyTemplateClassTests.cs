using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
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
        //Attributes
        //Modifiers
        //TypeName
        //Name
        //GetterVisibility
        //SetterVisibility
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
    }
}
