using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Objects.Extensions;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class NullableTestClassTests
    {
        [Fact]
        public void CanDetermineNullableReturnTypeOnMethod()
        {
            // Act
#pragma warning disable CS8604 // Possible null reference argument.
            var actual = typeof(NullableTestClass).GetMethod(nameof(NullableTestClass.GetValue)).ReturnTypeIsNullable();
#pragma warning restore CS8604 // Possible null reference argument.

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void CanDeterineNullableArgumentOnMethod()
        {
            // Act
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var actual = typeof(NullableTestClass).GetMethod(nameof(NullableTestClass.GetValue)).GetParameters().First().IsNullable();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void CanDetermineNullableField()
        {
            // Act
#pragma warning disable CS8604 // Possible null reference argument.
            var actual = typeof(NullableTestClass).GetField(nameof(NullableTestClass.ValueField)).IsNullable();
#pragma warning restore CS8604 // Possible null reference argument.

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void CanDetermineNullableProperty()
        {
            // Act
#pragma warning disable CS8604 // Possible null reference argument.
            var actual = typeof(NullableTestClass).GetProperty(nameof(NullableTestClass.Value)).IsNullable();
#pragma warning restore CS8604 // Possible null reference argument.

            // Assert
            actual.Should().BeTrue();
        }
    }
}
