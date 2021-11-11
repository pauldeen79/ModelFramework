using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;
using ModelFramework.Common.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace ModelFramework.Common.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class EnumerableOfIMetadataExtensionsTests
    {
        [Fact]
        public void CanGetValueWhenPresent()
        {
            // Arrange
            var lst = new[] { new Metadata("name", "value") }.OfType<IMetadata>();

            // Act
            var actual = lst.GetMetadataStringValue("name", "default");

            // Assert
            actual.Should().Be("value");
        }

        [Fact]
        public void CanGetDefaultValueWhenNotPresent()
        {
            // Arrange
            var lst = new[] { new Metadata("other name", "value") }.OfType<IMetadata>();

            // Act
            var actual = lst.GetMetadataStringValue("name", "default");

            // Assert
            actual.Should().Be("default");
        }

        [Fact]
        public void GetsFirstValueWhenPresent()
        {
            // Arrange
            var lst = new[] { new Metadata("name", "value"), new Metadata("name", "second value") }.OfType<IMetadata>();

            // Act
            var actual = lst.GetMetadataStringValue("name", "default");

            // Assert
            actual.Should().Be("value");
        }

        [Fact]
        public void CanGetMultipleValues()
        {
            // Arrange
            var lst = new[] { new Metadata("name", "value"), new Metadata("name", "second value") }.OfType<IMetadata>();

            // Act
            var actual = lst.GetMetadataStringValues("name");

            // Assert
            actual.Should().BeEquivalentTo(new[] { "value", "second value" });
        }
    }
}
