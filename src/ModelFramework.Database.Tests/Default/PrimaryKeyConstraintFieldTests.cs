using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Default;
using Xunit;

namespace ModelFramework.Database.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class PrimaryKeyConstraintFieldTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new PrimaryKeyConstraintField(false, "", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void ToString_Returns_Name_And_Direction_Descending()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintField(true, "Name", Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Name DESC");
        }

        [Fact]
        public void ToString_Returns_Name_And_Direction_Ascending()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintField(false, "Name", Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Name ASC");
        }
    }
}
