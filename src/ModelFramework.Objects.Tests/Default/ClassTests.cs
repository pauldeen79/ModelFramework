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
    public class ClassTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new Class("",
                                                        "",
                                                        default,
                                                        "",
                                                        default,
                                                        default,
                                                        default,
                                                        default,
                                                        Enumerable.Empty<string>(),
                                                        Enumerable.Empty<IClassField>(),
                                                        Enumerable.Empty<IClassProperty>(),
                                                        Enumerable.Empty<IClassMethod>(),
                                                        Enumerable.Empty<IClassConstructor>(),
                                                        Enumerable.Empty<IMetadata>(),
                                                        Enumerable.Empty<IAttribute>(),
                                                        Enumerable.Empty<IClass>(),
                                                        Enumerable.Empty<IEnum>(),
                                                        Enumerable.Empty<string>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Theory]
        [InlineData("Test", "", "Test")]
        [InlineData("Test", "MyNamespace", "MyNamespace.Test")]
        public void ToString_Returns_Correct_Result(string name, string @namespace, string expectedResult)
        {
            // Arrange
            var sut = new Class(name,
                                @namespace,
                                default,
                                "",
                                default,
                                default,
                                default,
                                default,
                                Enumerable.Empty<string>(),
                                Enumerable.Empty<IClassField>(),
                                Enumerable.Empty<IClassProperty>(),
                                Enumerable.Empty<IClassMethod>(),
                                Enumerable.Empty<IClassConstructor>(),
                                Enumerable.Empty<IMetadata>(),
                                Enumerable.Empty<IAttribute>(),
                                Enumerable.Empty<IClass>(),
                                Enumerable.Empty<IEnum>(),
                                Enumerable.Empty<string>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be(expectedResult);
        }
    }
}
