using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassGenerator_DefaultFieldTemplateTests
    {
        [Fact]
        public void GeneratesCodeBodyWithoutDefaultValueWhenNotSupplied()
        {
            // Arrange
            var model = new ClassFieldBuilder()
                .WithName("PropertyChanged")
                .WithTypeName("PropertyChangedEventHandler")
                .WithEvent()
                .WithVisibility(Visibility.Public)
                .Build();
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
            var model = new ClassFieldBuilder()
                .WithName("Name")
                .WithType(typeof(string))
                .Build();
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
            var model = new ClassFieldBuilder()
                .WithName("Name")
                .WithType(typeof(string))
                .WithDefaultValue("Hello world")
                .Build();
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
            var model = new ClassFieldBuilder()
                .WithName("Name")
                .WithType(typeof(string))
                .AddAttributes
                (
                    new AttributeBuilder().WithName("Attribute1"),
                    new AttributeBuilder().WithName("Attribute2"),
                    new AttributeBuilder().WithName("Attribute3")
                ).Build();
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
            var model = new ClassFieldBuilder()
                .WithName("Test")
                .WithType(typeof(string))
                .WithIsNullable()
                .Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model, rootAdditionalParameters: new { EnableNullableContext = true });

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        private string? Test;
");
        }
    }
}
