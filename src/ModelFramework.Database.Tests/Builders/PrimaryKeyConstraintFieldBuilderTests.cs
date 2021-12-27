using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Default;
using ModelFramework.Database.Builders;
using Xunit;

namespace ModelFramework.Database.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class PrimaryKeyConstraintFieldBuilderTests
    {
        [Fact]
        public void Can_Clear()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintFieldBuilder()
                .WithName("Name")
                .WithIsDescending()
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.Clear();

            // Assert
            actual.Name.Should().BeEmpty();
            actual.IsDescending.Should().BeFalse();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Clear_Metadata()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintFieldBuilder()
                .WithName("Name")
                .WithIsDescending()
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearMetadata();

            // Assert
            actual.Name.Should().Be("Name");
            actual.IsDescending.Should().BeTrue();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Add_Metadata()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintFieldBuilder().WithName("Name");

            // Act
            sut.AddMetadata(new[] { new MetadataBuilder() });
            sut.AddMetadata(new[] { new MetadataBuilder() }.AsEnumerable());
            sut.AddMetadata(new[] { new Metadata("Name", "Value") });
            sut.AddMetadata(new[] { new Metadata("Name", "Value") }.AsEnumerable());

            // Assert
            sut.Metadata.Should().HaveCount(4);
        }

        [Fact]
        public void Can_Construct_Builder_From_Entity_Instance()
        {
            // Arrange
            var instance = new PrimaryKeyConstraintFieldBuilder()
                .WithName("Name")
                .WithIsDescending()
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"))
                .Build();

            // Act
            var actual = new PrimaryKeyConstraintFieldBuilder(instance);

            // Assert
            actual.Name.Should().Be(instance.Name);
            actual.IsDescending.Should().Be(instance.IsDescending);
            actual.Metadata.Should().ContainSingle();
            actual.Metadata.First().Name.Should().Be(instance.Metadata.First().Name);
            actual.Metadata.First().Value.Should().Be(instance.Metadata.First().Value);
        }
    }
}
