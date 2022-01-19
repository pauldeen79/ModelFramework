using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using Xunit;

namespace ModelFramework.Objects.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class ClassConstructorBuilderTests
    {
        [Fact]
        public void Can_Set_ChainCall_With_Parameters()
        {
            // Act
            var sut = new ClassConstructorBuilder()
                .AddParameter("param1", typeof(int))
                .AddParameter("param2", typeof(string))
                .AddParameter("param3", typeof(bool));

            // Act
            var actual = sut.ChainCallToBaseUsingParameters();

            // Assert
            actual.ChainCall.Should().Be("base(param1, param2, param3)");
        }
    }
}
