using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using Xunit;

namespace ModelFramework.Objects.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class ClassTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new ClassBuilder().Build());

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Theory]
        [InlineData("Test", "", "Test")]
        [InlineData("Test", "MyNamespace", "MyNamespace.Test")]
        public void ToString_Returns_Correct_Result(string name, string @namespace, string expectedResult)
        {
            // Arrange
            var sut = new ClassBuilder().WithName(name).WithNamespace(@namespace).Build();

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be(expectedResult);
        }
    }
}
