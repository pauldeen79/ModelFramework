using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Extensions;
using Xunit;

namespace ModelFramework.Objects.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class TypeExtensionsTests
    {
        [Fact]
        public void WithoutGenerics_Throws_When_Type_Does_Not_Have_FullName()
        {
            // Arrange
            var sut = typeof(Nullable<>).GetGenericArguments()[0];
            var action = new Action(() => _ = sut.WithoutGenerics());

            // Act
            action.Should().Throw<ArgumentException>().WithMessage("Can't get typename without generics when the FullName of this type is null. Could not determine typename.");
        }

        [Fact]
        public void WithoutGenerics_Returns_Same_TypeName_When_Type_Is_Not_GenericType()
        {
            // Arrange
            var sut = typeof(string);

            // Act
            var actual = sut.WithoutGenerics();

            // Assert
            actual.Should().Be(typeof(string).FullName);
        }

        [Fact]
        public void WithoutGenerics_Strips_Generics_When_Type_Is_GenericType()
        {
            // Arrange
            var sut = typeof(int?);

            // Act
            var actual = sut.WithoutGenerics();

            // Assert
            actual.Should().Be("System.Nullable");
        }
    }
}
