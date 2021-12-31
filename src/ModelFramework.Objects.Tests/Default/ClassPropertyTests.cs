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
    public class ClassPropertyTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new ClassProperty("",
                                                                "System.String",
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                "",
                                                                Enumerable.Empty<IMetadata>(),
                                                                Enumerable.Empty<IAttribute>(),
                                                                Enumerable.Empty<ICodeStatement>(),
                                                                Enumerable.Empty<ICodeStatement>(),
                                                                Enumerable.Empty<ICodeStatement>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_TypeName()
        {
            // Arrange
            var action = new Action(() => _ = new ClassProperty("Test",
                                                                "",
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                default,
                                                                "",
                                                                Enumerable.Empty<IMetadata>(),
                                                                Enumerable.Empty<IAttribute>(),
                                                                Enumerable.Empty<ICodeStatement>(),
                                                                Enumerable.Empty<ICodeStatement>(),
                                                                Enumerable.Empty<ICodeStatement>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("typeName");
        }

        [Fact]
        public void ToString_Returns_Name()
        {
            // Arrange
            var sut = new ClassProperty("Test",
                                        "System.String",
                                        default,
                                        default,
                                        default,
                                        default,
                                        default,
                                        default,
                                        default,
                                        default,
                                        default,
                                        default,
                                        default,
                                        default,
                                        default,
                                        "",
                                        Enumerable.Empty<IMetadata>(),
                                        Enumerable.Empty<IAttribute>(),
                                        Enumerable.Empty<ICodeStatement>(),
                                        Enumerable.Empty<ICodeStatement>(),
                                        Enumerable.Empty<ICodeStatement>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Test");
        }
    }
}
