using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Builders;
using ModelFramework.Objects.Extensions;
using Xunit;

namespace ModelFramework.Objects.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ListOfMetadataBuilderExtensionsTests
    {
        [Fact]
        public void Can_Replace_Metadata_By_Name()
        {
            // Arrange
            var sut = new List<MetadataBuilder>
            {
                new MetadataBuilder().WithName("Test").WithValue("Old value")
            };

            // Act
            var actual = sut.Replace("Test", "New value");

            // Assert
            actual.Should().ContainSingle();
            actual.First().Name.Should().Be("Test");
            actual.First().Value.Should().Be("New value");
        }

        [Fact]
        public void When_Replacing_Metadata_Other_Metadata_Stays_There()
        {
            // Arrange
            var sut = new List<MetadataBuilder>
            {
                new MetadataBuilder().WithName("Test1").WithValue("Old value")
            };

            // Act
            var actual = sut.Replace("Test2", "New value");

            // Assert
            actual.Should().HaveCount(2);
            actual.First().Name.Should().Be("Test1");
            actual.First().Value.Should().Be("Old value");
            actual.Last().Name.Should().Be("Test2");
            actual.Last().Value.Should().Be("New value");
        }
    }
}
