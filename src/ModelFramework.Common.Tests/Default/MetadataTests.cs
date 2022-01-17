using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Common.Default;
using Xunit;

namespace ModelFramework.Common.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class MetadataTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new Metadata("Value", string.Empty));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void ToString_Returns_Name_Equals_Null_When_Value_Is_Null()
        {
            // Arrange
            var sut = new Metadata(null, "Name");

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("[Name] = NULL");
        }

        [Fact]
        public void ToString_Returns_Name_Equals_Value_When_Value_Is_Not_Null()
        {
            // Arrange
            var sut = new Metadata("Value", "Name");

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("[Name] = [Value]");
        }
    }
}
