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
    public class CheckConstraintBuilderTests
    {
        [Fact]
        public void Can_Build_Entity_With_All_Bells_And_Whistles()
        {
            // Arrange
            var sut = new CheckConstraintBuilder()
                .WithName("Name")
                .WithExpression("Expression")
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.Build();

            // Assert
            actual.Name.Should().Be(sut.Name);
            actual.Expression.Should().Be(sut.Expression);
            actual.Metadata.Should().ContainSingle();
            actual.Metadata.First().Name.Should().Be(sut.Metadata.First().Name);
            actual.Metadata.First().Value.Should().Be(sut.Metadata.First().Value);
        }

        [Fact]
        public void Can_Clear()
        {
            // Arrange
            var sut = new CheckConstraintBuilder()
                .WithName("Name")
                .WithExpression("Expression")
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.Clear();

            // Assert
            actual.Name.Should().BeEmpty();
            actual.Expression.Should().BeEmpty();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Clear_Metadata()
        {
            // Arrange
            var sut = new CheckConstraintBuilder()
                .WithName("Name")
                .WithExpression("Expression")
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearMetadata();

            // Assert
            actual.Name.Should().Be("Name");
            actual.Expression.Should().Be("Expression");
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Add_Metadata()
        {
            // Arrange
            var sut = new CheckConstraintBuilder()
                .WithName("Name")
                .WithExpression("Expression");

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
            var instance = new CheckConstraintBuilder()
                .WithName("Test")
                .WithExpression("Expression")
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"))
                .Build();

            // Act
            var actual = new CheckConstraintBuilder(instance);

            // Assert
            actual.Name.Should().Be(instance.Name);
            actual.Expression.Should().Be(instance.Expression);
            actual.Metadata.Should().ContainSingle();
            actual.Metadata.First().Name.Should().Be(instance.Metadata.First().Name);
            actual.Metadata.First().Value.Should().Be(instance.Metadata.First().Value);
        }
    }
}
