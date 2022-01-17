using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Extensions;
using Xunit;

namespace ModelFramework.Objects.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ClassPropertyExtensionsTests
    {
        [Fact]
        public void Can_Specify_Custom_Modifiers_For_Getter()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("Test")
                                                .WithTypeName("System.Int32")
                                                .WithCustomGetterModifiers("internal")
                                                .Build();

            // Act
            var actual = sut.GetGetterModifiers();

            // Assert
            actual.Should().Be("internal ");
        }

        [Fact]
        public void Can_Specify_Custom_Modifiers_For_Setter()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("Test")
                                                .WithTypeName("System.Int32")
                                                .WithCustomSetterModifiers("internal")
                                                .Build();

            // Act
            var actual = sut.GetSetterModifiers();

            // Assert
            actual.Should().Be("internal ");
        }

        [Fact]
        public void Can_Specify_Custom_Modifiers_For_Initializer()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("Test")
                                                .WithTypeName("System.Int32")
                                                .WithCustomInitializerModifiers("internal")
                                                .Build();

            // Act
            var actual = sut.GetInitializerModifiers();

            // Assert
            actual.Should().Be("internal ");
        }
    }
}
