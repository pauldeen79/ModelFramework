using System.Linq;
using FluentAssertions;
using ModelFramework.Objects.Extensions;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    public class NullableTestClassTests
    {
        [Fact]
        public void CanDetermineNullableReturnTypeOnMethod()
        {
            // Act
            var actual = typeof(NullableTestClass).GetMethod(nameof(NullableTestClass.GetValue)).ReturnTypeIsNullable();

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void CanDeterineNullableArgumentOnMethod()
        {
            // Act
            var actual = typeof(NullableTestClass).GetMethod(nameof(NullableTestClass.GetValue)).GetParameters().First().IsNullable();

            // Assert
            actual.Should().BeTrue();
        }
    }
}
