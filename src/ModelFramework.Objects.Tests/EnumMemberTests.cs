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
    public class EnumMemberTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new EnumMember(null,
                                                             Enumerable.Empty<IAttribute>(),
                                                             string.Empty,
                                                             Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void ToString_Returns_Name_When_Value_Is_Null()
        {
            // Arrange
            var sut = new EnumMember(null,
                                     Enumerable.Empty<IAttribute>(),
                                     "Test",
                                     Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Test");
        }

        [Fact]
        public void ToString_Returns_Name_And_Value_When_Value_Is_Not_Null()
        {
            // Arrange
            var sut = new EnumMember(1,
                                     Enumerable.Empty<IAttribute>(),
                                     "Test",
                                     Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("[Test] = [1]");
        }
    }
}
