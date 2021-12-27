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
    public class IndexBuilderTests
    {
        [Fact]
        public void Can_Clear()
        {
            // Arrange
            var sut = new IndexBuilder()
                .WithName("Name")
                .WithFileGroupName("PRIMARY")
                .WithUnique()
                .AddFields(new IndexFieldBuilder().WithName("MyField"))
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.Clear();

            // Assert
            actual.Name.Should().BeEmpty();
            actual.FileGroupName.Should().BeEmpty();
            actual.Unique.Should().BeFalse();
            actual.Fields.Should().BeEmpty();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Clear_Metadata()
        {
            // Arrange
            var sut = new IndexBuilder()
                .WithName("Name")
                .WithFileGroupName("PRIMARY")
                .WithUnique()
                .AddFields(new IndexFieldBuilder().WithName("MyField"))
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearMetadata();

            // Assert
            actual.Name.Should().Be("Name");
            actual.FileGroupName.Should().Be("PRIMARY");
            actual.Unique.Should().BeTrue();
            actual.Fields.Should().ContainSingle();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Add_Metadata()
        {
            // Arrange
            var sut = new IndexBuilder().WithName("Name");

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
            var instance = new IndexBuilder()
                .WithName("Name")
                .WithFileGroupName("PRIMARY")
                .WithUnique()
                .AddFields(new IndexFieldBuilder().WithName("MyField"))
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"))
                .Build();

            // Act
            var actual = new IndexBuilder(instance);

            // Assert
            actual.Name.Should().Be(instance.Name);
            actual.FileGroupName.Should().Be(instance.FileGroupName);
            actual.Unique.Should().Be(instance.Unique);
            actual.Metadata.Should().ContainSingle();
            actual.Metadata.First().Name.Should().Be(instance.Metadata.First().Name);
            actual.Metadata.First().Value.Should().Be(instance.Metadata.First().Value);
            actual.Fields.Should().ContainSingle();
            actual.Fields.First().Name.Should().Be(instance.Fields.First().Name);
        }
    }
}
