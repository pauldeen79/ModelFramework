using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Common.Builders;
using Xunit;

namespace ModelFramework.Common.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class MetadataBuilderTests
    {
        [Fact]
        public void CanClearInstance()
        {
            // Arrange
            var sut = new MetadataBuilder().WithName("Name").WithValue("Value");

            // Act
            sut.Clear();

            // Asset
            sut.Name.Should().BeEmpty();
            sut.Value.Should().BeNull();
        }

        [Fact]
        public void CanBuildInstance()
        {
            // Arrange
            var sut = new MetadataBuilder().WithName("Name").WithValue("Value");

            // Act
            var actual = sut.Build();

            // Asset
            actual.Name.Should().Be(sut.Name);
            actual.Value.Should().Be(sut.Value);
        }
    }
}
