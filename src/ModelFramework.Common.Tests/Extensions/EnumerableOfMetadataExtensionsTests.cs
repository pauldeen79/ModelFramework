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
    public class EnumerableOfMetadataExtensionsTests
    {
        [Fact]
        public void CanGetValueWhenPresent()
        {
            // Arrange
            var lst = new[] { new Metadata("name", "value") }.OfType<IMetadata>();

            // Act
            var actual = lst.GetStringValue("name", "default");

            // Assert
            actual.Should().Be("value");
        }

        [Fact]
        public void CanGetDefaultValueWhenNotPresent()
        {
            // Arrange
            var lst = new[] { new Metadata("other name", "value") }.OfType<IMetadata>();

            // Act
            var actual = lst.GetStringValue("name", "default");

            // Assert
            actual.Should().Be("default");
        }

        [Fact]
        public void GetsFirstValueWhenPresent()
        {
            // Arrange
            var lst = new[] { new Metadata("name", "value"), new Metadata("name", "second value") }.OfType<IMetadata>();

            // Act
            var actual = lst.GetStringValue("name", "default");

            // Assert
            actual.Should().Be("value");
        }

        [Fact]
        public void CanGetMultipleValues()
        {
            // Arrange
            var lst = new[] { new Metadata("name", "value"), new Metadata("name", "second value") }.OfType<IMetadata>();

            // Act
            var actual = lst.GetStringValues("name");

            // Assert
            actual.Should().BeEquivalentTo(new[] { "value", "second value" });
        }

        [Fact]
        public void CanGetBooleanValue()
        {
            // Arrange
            var lst = new[] { new Metadata("name", true), new Metadata("name", false) }.OfType<IMetadata>();

            // Act
            var actual = lst.GetBooleanValue("name");

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void GetBooleanValueWithDefaultValueReturnsDefaultWhenNotFound()
        {
            // Arrange
            var lst = new[] { new Metadata("name", true), new Metadata("name", false) }.OfType<IMetadata>();

            // Act
            var actual = lst.GetBooleanValue("wrongname", true);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void GetBooleanValueWithDefaultValueDelegateReturnsDefaultWhenNotFound()
        {
            // Arrange
            var lst = new[] { new Metadata("name", true), new Metadata("name", false) }.OfType<IMetadata>();

            // Act
            var actual = lst.GetBooleanValue("name", () => true);

            // Assert
            actual.Should().BeTrue();
        }
    }
}
