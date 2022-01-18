using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using Xunit;

namespace ModelFramework.Objects.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class AttributeBuilderTests
    {
        [Fact]
        public void WithName_Sets_FullName_Of_Specified_Type()
        {
            // Arrange
            var sut = new AttributeBuilder();

            // Act
            var actual = sut.WithName(typeof(MinLengthAttribute)).AddParameters(new AttributeParameterBuilder().WithValue(1));

            // Assert
            actual.Name.Should().Be("System.ComponentModel.DataAnnotations.MinLengthAttribute");
            actual.Parameters.Should().ContainSingle();
            actual.Parameters.First().Value.Should().Be(1);
        }

        [Fact]
        public void Bla()
        {
            // Arrange
            var sut = new AttributeBuilder();

            // Act
            var actual = sut.AddNameAndParameter("System.ComponentModel.ReadOnly", true);

            // Assert
            actual.Name.Should().Be("System.ComponentModel.ReadOnly");
            actual.Parameters.Should().ContainSingle();
            actual.Parameters.First().Value.Should().Be(true);
        }
    }
}
