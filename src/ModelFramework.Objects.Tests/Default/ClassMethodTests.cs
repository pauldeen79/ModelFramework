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
    public class ClassMethodTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new ClassMethod("",
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
                                                              "",
                                                              Enumerable.Empty<IParameter>(),
                                                              Enumerable.Empty<IAttribute>(),
                                                              Enumerable.Empty<ICodeStatement>(),
                                                              Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void ToString_Returns_Name()
        {
            // Arrange
            var sut = new ClassMethod("Test",
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
                                      "",
                                      Enumerable.Empty<IParameter>(),
                                      Enumerable.Empty<IAttribute>(),
                                      Enumerable.Empty<ICodeStatement>(),
                                      Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Test");
        }
    }
}
