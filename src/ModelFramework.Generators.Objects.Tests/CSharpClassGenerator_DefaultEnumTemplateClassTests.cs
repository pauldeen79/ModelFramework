using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class Enum_DefaultClassTests
    {
        [Fact]
        public void GeneratesCodeBody()
        {
            // Arrange
            var model = new Enum("MyEnum", new[] { new EnumMember("Member1"), new EnumMember("Member2"), new EnumMember("Member3") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public enum MyEnum
        {
            Member1,
            Member2,
            Member3
        }
");
        }

        [Fact]
        public void GeneratesAttributes()
        {
            // Arrange
            var model = new Enum("MyEnum", new[] { new EnumMember("Member1"), new EnumMember("Member2"), new EnumMember("Member3") }, attributes: new[] { new Attribute("Attribute1"), new Attribute("Attribute2"), new Attribute("Attribute3") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        [Attribute1]
        [Attribute2]
        [Attribute3]
        public enum MyEnum
        {
            Member1,
            Member2,
            Member3
        }
");
        }

        [Fact]
        public void GeneratesInternalEnum()
        {
            // Arrange
            var model = new Enum("MyEnum", new[] { new EnumMember("Member1"), new EnumMember("Member2"), new EnumMember("Member3") }, Visibility.Internal);
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        internal enum MyEnum
        {
            Member1,
            Member2,
            Member3
        }
");
        }
    }
}
