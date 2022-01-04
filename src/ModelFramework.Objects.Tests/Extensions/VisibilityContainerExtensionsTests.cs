using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Extensions;
using Xunit;

namespace ModelFramework.Objects.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class VisibilityContainerExtensionsTests
    {
        [Fact]
        public void Can_Specify_Custom_Modifiers()
        {
            // Arrange
            var sut = new ClassFieldBuilder().WithName("Test")
                                             .WithType(typeof(int))
                                             .WithCustomModifiers("internal readonly")
                                             .Build();

            // Act
            var actual = sut.GetModifiers();

            // Assert
            actual.Should().Be("internal readonly ");
        }

        [Fact]
        public void Can_Specify_Constant_On_Field()
        {
            // Arrange
            var sut = new ClassFieldBuilder().WithName("Test")
                                             .WithType(typeof(int))
                                            .WithConstant()
                                            .Build();

            // Act
            var actual = sut.GetModifiers();

            // Assert
            actual.Should().Be("private const ");
        }
    }
}
