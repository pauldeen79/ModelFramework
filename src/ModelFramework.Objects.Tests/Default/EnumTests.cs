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
    public class EnumTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new Objects.Default.Enum(string.Empty,
                                                                       new[] { new EnumMember("First", 1, Enumerable.Empty<IAttribute>(), Enumerable.Empty<IMetadata>()) },
                                                                       Visibility.Public,
                                                                       Enumerable.Empty<IAttribute>(),
                                                                       Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_Members()
        {
            // Arrange
            var action = new Action(() => _ = new Objects.Default.Enum("Test",
                                                                       Enumerable.Empty<IEnumMember>(),
                                                                       Visibility.Public,
                                                                       Enumerable.Empty<IAttribute>(),
                                                                       Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("members");
        }

        [Fact]
        public void ToString_Returns_Name()
        {
            // Arrange
            var sut = new Objects.Default.Enum("Test",
                                               new[] { new EnumMember("First", 1, Enumerable.Empty<IAttribute>(), Enumerable.Empty<IMetadata>()) },
                                               Visibility.Public,
                                               Enumerable.Empty<IAttribute>(),
                                               Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Test");
        }
    }
}
