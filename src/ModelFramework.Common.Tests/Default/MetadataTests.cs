using System;
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
        public void Ctor_Throws_On_Null_Name()
        {
            // Arrange
            var action = new Action(() => _ = new Metadata(string.Empty, "Value"));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void ToString_Returns_Name_Equals_Null_When_Value_Is_Null()
        {
            // Arrange
            var sut = new Metadata("Name", null);

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("[Name] = NULL");
        }

        [Fact]
        public void ToString_Returns_Name_Equals_Value_When_Value_Is_Not_Null()
        {
            // Arrange
            var sut = new Metadata("Name", "Value");

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("[Name] = [Value]");
        }
    }
}
