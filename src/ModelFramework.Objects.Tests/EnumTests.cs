using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using Xunit;

namespace ModelFramework.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class EnumTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new Enum(new[] { new EnumMember(1, Enumerable.Empty<IAttribute>(), "First", Enumerable.Empty<IMetadata>()) },
                                                       Enumerable.Empty<IAttribute>(),
                                                       Enumerable.Empty<IMetadata>(),
                                                       string.Empty,
                                                       Visibility.Public));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_Members()
        {
            // Arrange
            var action = new Action(() => _ = new Enum(Enumerable.Empty<IEnumMember>(),
                                                       Enumerable.Empty<IAttribute>(),
                                                       Enumerable.Empty<IMetadata>(),
                                                       "Test",
                                                       Visibility.Public));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Enum should have at least one member");
        }

        [Fact]
        public void ToString_Returns_Name()
        {
            // Arrange
            var sut = new Enum(new[] { new EnumMember(1, Enumerable.Empty<IAttribute>(), "First", Enumerable.Empty<IMetadata>()) },
                               Enumerable.Empty<IAttribute>(),
                               Enumerable.Empty<IMetadata>(),
                               "Test",
                               Visibility.Public);

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Test");
        }
    }
}
