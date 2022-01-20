using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using Xunit;

namespace ModelFramework.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class ClassMethodTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new ClassMethodBuilder().Build());

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void ToString_Returns_Name()
        {
            // Arrange
            var sut = new ClassMethodBuilder().WithName("Test").Build();

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Test");
        }
    }
}
