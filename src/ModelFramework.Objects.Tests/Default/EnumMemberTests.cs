using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using Xunit;

namespace ModelFramework.Objects.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class EnumMemberTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new EnumMember(string.Empty,
                                                             null,
                                                             Enumerable.Empty<IAttribute>(),
                                                             Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void ToString_Returns_Name_When_Value_Is_Null()
        {
            // Arrange
            var sut = new EnumMember("Test",
                                     null,
                                     Enumerable.Empty<IAttribute>(),
                                     Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Test");
        }

        [Fact]
        public void ToString_Returns_Name_And_Value_When_Value_Is_Not_Null()
        {
            // Arrange
            var sut = new EnumMember("Test",
                                     1,
                                     Enumerable.Empty<IAttribute>(),
                                     Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("[Test] = [1]");
        }
    }
}
