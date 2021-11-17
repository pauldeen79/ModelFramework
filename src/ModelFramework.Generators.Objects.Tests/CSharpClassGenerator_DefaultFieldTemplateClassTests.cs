using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class Field_DefaultClassTests
    {
        [Fact]
        public void GeneratesCodeBodyWithoutDefaultValueWhenNotSupplied()
        {
            // Arrange
            var model = new ClassField("PropertyChanged", "PropertyChangedEventHandler", @event: true, visibility: ModelFramework.Objects.Contracts.Visibility.Public);
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public event PropertyChangedEventHandler PropertyChanged;
");
        }

        [Fact]
        public void GeneratesCodeBodyForEvent()
        {
            // Arrange
            var model = new ClassField("Name", "string");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        private string Name;
");
        }

        [Fact]
        public void GeneratesCodeBodyWithDefaultValueWhenSupplied()
        {
            // Arrange
            var model = new ClassField("Name", "string", defaultValue: "Hello world");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        private string Name = @""Hello world"";
");
        }

        [Fact]
        public void GeneratesAttributes()
        {
            // Arrange
            var model = new ClassField("Name", "string", attributes: new[] { new Attribute("Attribute1"), new Attribute("Attribute2"), new Attribute("Attribute3") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        [Attribute1]
        [Attribute2]
        [Attribute3]
        private string Name;
");
        }

        [Fact]
        public void GeneratesNullableField()
        {
            // Arrange
            var model = new ClassField("Test", typeof(string).FullName, isNullable: true);
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model, rootAdditionalParameters: new { EnableNullableContext = true });

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        private string? Test;
");
        }
    }
}
