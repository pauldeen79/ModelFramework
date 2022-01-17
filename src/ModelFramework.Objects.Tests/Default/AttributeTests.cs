using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using Xunit;

namespace ModelFramework.Objects.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class AttributeTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new Objects.Default.Attribute(Enumerable.Empty<IAttributeParameter>(), Enumerable.Empty<IMetadata>(), string.Empty));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void ToString_Returns_Name()
        {
            // Arrange
            var sut = new Objects.Default.Attribute(Enumerable.Empty<IAttributeParameter>(), Enumerable.Empty<IMetadata>(), "Test");

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Test");
        }
    }
}
