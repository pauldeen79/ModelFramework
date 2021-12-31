using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Default;
using Xunit;

namespace ModelFramework.Objects.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class AttributeParameterTests
    {
        [Fact]
        public void ToString_Returns_Name_When_Name_Is_Not_Empty()
        {
            // Arrange
            var sut = new AttributeParameter("TestValue", "TestName", Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("TestName");
        }

        [Fact]
        public void ToString_Returns_Value_When_Name_Is_Empty()
        {
            // Arrange
            var sut = new AttributeParameter("TestValue", "", Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("TestValue");
        }

        [Fact]
        public void ToString_Returns_String_Empty_When_Name_And_Value_Are_Both_Empty()
        {
            // Arrange
            var sut = new AttributeParameter("", "", Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().BeEmpty();
        }
    }
}
