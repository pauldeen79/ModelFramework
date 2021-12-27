using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;
using ModelFramework.Database.Builders;
using ModelFramework.Database.Default;
using Xunit;

namespace ModelFramework.Database.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class PrimaryKeyConstraintBuilderTests
    {
        [Fact]
        public void Can_Clear()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintBuilder()
                .WithName("Name")
                .WithFileGroupName("PRIMARY")
                .AddFields(new PrimaryKeyConstraintFieldBuilder().WithName("MyField"))
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.Clear();

            // Assert
            actual.Name.Should().BeEmpty();
            actual.FileGroupName.Should().BeEmpty();
            actual.Fields.Should().BeEmpty();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Clear_Metadata()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintBuilder()
                .WithName("Name")
                .WithFileGroupName("PRIMARY")
                .AddFields(new PrimaryKeyConstraintFieldBuilder().WithName("MyField"))
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearMetadata();

            // Assert
            actual.Name.Should().Be("Name");
            actual.FileGroupName.Should().Be("PRIMARY");
            actual.Fields.Should().ContainSingle();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Add_Metadata()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintBuilder().WithName("Name");

            // Act
            sut.AddMetadata(new[] { new MetadataBuilder() });
            sut.AddMetadata(new[] { new MetadataBuilder() }.AsEnumerable());
            sut.AddMetadata(new[] { new Metadata("Name", "Value") });
            sut.AddMetadata(new[] { new Metadata("Name", "Value") }.AsEnumerable());

            // Assert
            sut.Metadata.Should().HaveCount(4);
        }

        [Fact]
        public void Can_Clear_Fields()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintBuilder()
                .WithName("Name")
                .WithFileGroupName("PRIMARY")
                .AddFields(new PrimaryKeyConstraintFieldBuilder().WithName("MyField"))
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearFields();

            // Assert
            actual.Name.Should().Be("Name");
            actual.FileGroupName.Should().Be("PRIMARY");
            actual.Fields.Should().BeEmpty();
            actual.Metadata.Should().ContainSingle();
        }

        [Fact]
        public void Can_Add_Fields()
        {
            // Arrange
            var sut = new PrimaryKeyConstraintBuilder().WithName("Name");

            // Act
            sut.AddFields(new[] { new PrimaryKeyConstraintFieldBuilder() });
            sut.AddFields(new[] { new PrimaryKeyConstraintFieldBuilder() }.AsEnumerable());
            sut.AddFields(new[] { new PrimaryKeyConstraintField("Name", false, Enumerable.Empty<IMetadata>()) });
            sut.AddFields(new[] { new PrimaryKeyConstraintField("Name", false, Enumerable.Empty<IMetadata>()) }.AsEnumerable());

            // Assert
            sut.Fields.Should().HaveCount(4);
        }
        [Fact]
        public void Can_Construct_Builder_From_Entity_Instance()
        {
            // Arrange
            var instance = new PrimaryKeyConstraintBuilder()
                .WithName("Name")
                .WithFileGroupName("PRIMARY")
                .AddFields(new PrimaryKeyConstraintFieldBuilder().WithName("MyField"))
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"))
                .Build();

            // Act
            var actual = new PrimaryKeyConstraintBuilder(instance);

            // Assert
            actual.Name.Should().Be(instance.Name);
            actual.FileGroupName.Should().Be(instance.FileGroupName);
            actual.Metadata.Should().ContainSingle();
            actual.Metadata.First().Name.Should().Be(instance.Metadata.First().Name);
            actual.Metadata.First().Value.Should().Be(instance.Metadata.First().Value);
            actual.Fields.Should().ContainSingle();
            actual.Fields.First().Name.Should().Be(instance.Fields.First().Name);
        }
    }
}
