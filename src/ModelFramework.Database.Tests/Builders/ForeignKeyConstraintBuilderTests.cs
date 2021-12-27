using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;
using ModelFramework.Database.Builders;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using Xunit;

namespace ModelFramework.Database.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class ForeignKeyConstraintBuilderTests
    {
        [Fact]
        public void Can_Clear()
        {
            // Arrange
            var sut = new ForeignKeyConstraintBuilder()
                .WithName("Name")
                .WithForeignTableName("ForeignTableName")
                .WithCascadeUpdate(CascadeAction.SetNull)
                .WithCascadeDelete(CascadeAction.SetDefault)
                .AddLocalFields(new ForeignKeyConstraintFieldBuilder())
                .AddForeignFields(new ForeignKeyConstraintFieldBuilder())
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.Clear();

            // Assert
            actual.Name.Should().BeEmpty();
            actual.ForeignTableName.Should().BeEmpty();
            actual.CascadeUpdate.Should().Be(default);
            actual.CascadeDelete.Should().Be(default);
            actual.LocalFields.Should().BeEmpty();
            actual.ForeignFields.Should().BeEmpty();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Clear_Metadata()
        {
            // Arrange
            var sut = new ForeignKeyConstraintBuilder()
                .WithName("Name")
                .WithForeignTableName("ForeignTableName")
                .WithCascadeUpdate(CascadeAction.SetNull)
                .WithCascadeDelete(CascadeAction.SetDefault)
                .AddLocalFields(new ForeignKeyConstraintFieldBuilder())
                .AddForeignFields(new ForeignKeyConstraintFieldBuilder())
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearMetadata();

            // Assert
            actual.Name.Should().Be("Name");
            actual.ForeignTableName.Should().Be("ForeignTableName");
            actual.CascadeUpdate.Should().Be(CascadeAction.SetNull);
            actual.CascadeDelete.Should().Be(CascadeAction.SetDefault);
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Add_Metadata()
        {
            // Arrange
            var sut = new ForeignKeyConstraintBuilder()
                .WithName("Name")
                .WithForeignTableName("ForeignTableName")
                .WithCascadeUpdate(CascadeAction.SetNull)
                .WithCascadeDelete(CascadeAction.SetDefault)
                .AddLocalFields(new ForeignKeyConstraintFieldBuilder())
                .AddForeignFields(new ForeignKeyConstraintFieldBuilder());

            // Act
            sut.AddMetadata(new[] { new MetadataBuilder() });
            sut.AddMetadata(new[] { new MetadataBuilder() }.AsEnumerable());
            sut.AddMetadata(new[] { new Metadata("Name", "Value") });
            sut.AddMetadata(new[] { new Metadata("Name", "Value") }.AsEnumerable());

            // Assert
            sut.Metadata.Should().HaveCount(4);
        }

        [Fact]
        public void Can_Clear_LocalFields()
        {
            // Arrange
            var sut = new ForeignKeyConstraintBuilder()
                .WithName("Name")
                .WithForeignTableName("ForeignTableName")
                .WithCascadeUpdate(CascadeAction.SetNull)
                .WithCascadeDelete(CascadeAction.SetDefault)
                .AddLocalFields(new ForeignKeyConstraintFieldBuilder())
                .AddForeignFields(new ForeignKeyConstraintFieldBuilder())
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearLocalFields();

            // Assert
            actual.Name.Should().Be("Name");
            actual.ForeignTableName.Should().Be("ForeignTableName");
            actual.CascadeUpdate.Should().Be(CascadeAction.SetNull);
            actual.CascadeDelete.Should().Be(CascadeAction.SetDefault);
            actual.LocalFields.Should().BeEmpty();
            actual.ForeignFields.Should().ContainSingle();
            actual.Metadata.Should().ContainSingle();
        }

        [Fact]
        public void Can_Add_LocalFields()
        {
            // Arrange
            var sut = new ForeignKeyConstraintBuilder()
                .WithName("Name")
                .WithForeignTableName("ForeignTableName")
                .WithCascadeUpdate(CascadeAction.SetNull)
                .WithCascadeDelete(CascadeAction.SetDefault)
                .AddLocalFields(new ForeignKeyConstraintFieldBuilder())
                .AddForeignFields(new ForeignKeyConstraintFieldBuilder());

            // Act
            sut.AddLocalFields(new[] { new ForeignKeyConstraintFieldBuilder() });
            sut.AddLocalFields(new[] { new ForeignKeyConstraintFieldBuilder() }.AsEnumerable());
            sut.AddLocalFields(new[] { new ForeignKeyConstraintField("Name", Enumerable.Empty<IMetadata>()) });
            sut.AddLocalFields(new[] { new ForeignKeyConstraintField("Name", Enumerable.Empty<IMetadata>()) }.AsEnumerable());

            // Assert
            sut.LocalFields.Should().HaveCount(5);
        }

        [Fact]
        public void Can_Clear_ForeignFields()
        {
            // Arrange
            var sut = new ForeignKeyConstraintBuilder()
                .WithName("Name")
                .WithForeignTableName("ForeignTableName")
                .WithCascadeUpdate(CascadeAction.SetNull)
                .WithCascadeDelete(CascadeAction.SetDefault)
                .AddLocalFields(new ForeignKeyConstraintFieldBuilder())
                .AddForeignFields(new ForeignKeyConstraintFieldBuilder())
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearForeignFields();

            // Assert
            actual.Name.Should().Be("Name");
            actual.ForeignTableName.Should().Be("ForeignTableName");
            actual.CascadeUpdate.Should().Be(CascadeAction.SetNull);
            actual.CascadeDelete.Should().Be(CascadeAction.SetDefault);
            actual.LocalFields.Should().ContainSingle();
            actual.ForeignFields.Should().BeEmpty();
            actual.Metadata.Should().ContainSingle();
        }

        [Fact]
        public void Can_Add_ForeignFields()
        {
            // Arrange
            var sut = new ForeignKeyConstraintBuilder()
                .WithName("Name")
                .WithForeignTableName("ForeignTableName")
                .WithCascadeUpdate(CascadeAction.SetNull)
                .WithCascadeDelete(CascadeAction.SetDefault)
                .AddLocalFields(new ForeignKeyConstraintFieldBuilder())
                .AddForeignFields(new ForeignKeyConstraintFieldBuilder());

            // Act
            sut.AddForeignFields(new[] { new ForeignKeyConstraintFieldBuilder() });
            sut.AddForeignFields(new[] { new ForeignKeyConstraintFieldBuilder() }.AsEnumerable());
            sut.AddForeignFields(new[] { new ForeignKeyConstraintField("Name", Enumerable.Empty<IMetadata>()) });
            sut.AddForeignFields(new[] { new ForeignKeyConstraintField("Name", Enumerable.Empty<IMetadata>()) }.AsEnumerable());

            // Assert
            sut.ForeignFields.Should().HaveCount(5);
        }

        [Fact]
        public void Can_Construct_Builder_From_Entity_Instance()
        {
            // Arrange
            var instance = new ForeignKeyConstraintBuilder()
                .WithName("Name")
                .WithForeignTableName("ForeignTableName")
                .WithCascadeUpdate(CascadeAction.SetNull)
                .WithCascadeDelete(CascadeAction.SetDefault)
                .AddLocalFields(new ForeignKeyConstraintFieldBuilder().WithName("Local"))
                .AddForeignFields(new ForeignKeyConstraintFieldBuilder().WithName("Foreign"))
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"))
                .Build();

            // Act
            var actual = new ForeignKeyConstraintBuilder(instance);

            // Assert
            actual.Name.Should().Be(instance.Name);
            actual.ForeignTableName.Should().Be(instance.ForeignTableName);
            actual.CascadeUpdate.Should().Be(instance.CascadeUpdate);
            actual.CascadeDelete.Should().Be(instance.CascadeDelete);
            actual.LocalFields.Should().ContainSingle();
            actual.LocalFields.First().Name.Should().Be(instance.LocalFields.First().Name);
            actual.ForeignFields.Should().ContainSingle();
            actual.ForeignFields.First().Name.Should().Be(instance.ForeignFields.First().Name);
            actual.Metadata.Should().ContainSingle();
            actual.Metadata.First().Name.Should().Be(instance.Metadata.First().Name);
            actual.Metadata.First().Value.Should().Be(instance.Metadata.First().Value);
        }
    }
}
