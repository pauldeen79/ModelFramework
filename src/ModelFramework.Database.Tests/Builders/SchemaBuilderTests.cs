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
    public class SchemaBuilderTests
    {
        [Fact]
        public void Can_Clear()
        {
            // Arrange
            var sut = new SchemaBuilder()
                .WithName("Name")
                .AddStoredProcedures(new StoredProcedureBuilder())
                .AddTables(new TableBuilder())
                .AddViews(new ViewBuilder())
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.Clear();

            // Assert
            actual.Name.Should().BeEmpty();
            actual.StoredProcedures.Should().BeEmpty();
            actual.Tables.Should().BeEmpty();
            actual.Views.Should().BeEmpty();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Clear_Metadata()
        {
            // Arrange
            var sut = new SchemaBuilder()
                .WithName("Name")
                .AddStoredProcedures(new StoredProcedureBuilder())
                .AddTables(new TableBuilder())
                .AddViews(new ViewBuilder())
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearMetadata();

            // Assert
            actual.Name.Should().Be("Name");
            actual.StoredProcedures.Should().ContainSingle();
            actual.Tables.Should().ContainSingle();
            actual.Views.Should().ContainSingle();
            actual.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Can_Add_Metadata()
        {
            // Arrange
            var sut = new SchemaBuilder().WithName("Name");

            // Act
            sut.AddMetadata(new[] { new MetadataBuilder() });
            sut.AddMetadata(new[] { new MetadataBuilder() }.AsEnumerable());
            sut.AddMetadata(new[] { new Metadata("Name", "Value") });
            sut.AddMetadata(new[] { new Metadata("Name", "Value") }.AsEnumerable());

            // Assert
            sut.Metadata.Should().HaveCount(4);
        }

        [Fact]
        public void Can_Clear_StoredProcedures()
        {
            // Arrange
            var sut = new SchemaBuilder()
                .WithName("Name")
                .AddStoredProcedures(new StoredProcedureBuilder())
                .AddTables(new TableBuilder())
                .AddViews(new ViewBuilder())
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearStoredProcedures();

            // Assert
            actual.Name.Should().Be("Name");
            actual.StoredProcedures.Should().BeEmpty();
            actual.Tables.Should().ContainSingle();
            actual.Views.Should().ContainSingle();
            actual.Metadata.Should().ContainSingle();
        }

        [Fact]
        public void Can_Add_StoredProcedures()
        {
            // Arrange
            var sut = new SchemaBuilder().WithName("Name");

            // Act
            sut.AddStoredProcedures(new[] { new StoredProcedureBuilder() });
            sut.AddStoredProcedures(new[] { new StoredProcedureBuilder() }.AsEnumerable());
            sut.AddStoredProcedures(new[] { new StoredProcedure("Name", Enumerable.Empty<IStoredProcedureParameter>(), Enumerable.Empty<ISqlStatement>(), Enumerable.Empty<IMetadata>()) });
            sut.AddStoredProcedures(new[] { new StoredProcedure("Name", Enumerable.Empty<IStoredProcedureParameter>(), Enumerable.Empty<ISqlStatement>(), Enumerable.Empty<IMetadata>()) }.AsEnumerable());

            // Assert
            sut.StoredProcedures.Should().HaveCount(4);
        }

        [Fact]
        public void Can_Clear_Tables()
        {
            // Arrange
            var sut = new SchemaBuilder()
                .WithName("Name")
                .AddStoredProcedures(new StoredProcedureBuilder())
                .AddTables(new TableBuilder())
                .AddViews(new ViewBuilder())
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"));

            // Act
            var actual = sut.ClearTables();

            // Assert
            actual.Name.Should().Be("Name");
            actual.StoredProcedures.Should().ContainSingle();
            actual.Tables.Should().BeEmpty();
            actual.Views.Should().ContainSingle();
            actual.Metadata.Should().ContainSingle();
        }

        [Fact]
        public void Can_Add_Tables()
        {
            // Arrange
            var sut = new SchemaBuilder().WithName("Name");

            // Act
            sut.AddTables(new[] { new TableBuilder() });
            sut.AddTables(new[] { new TableBuilder() }.AsEnumerable());
            sut.AddTables(new[] { new Table("Name", "", Enumerable.Empty<ITableField>(), Enumerable.Empty<IPrimaryKeyConstraint>(), Enumerable.Empty<IUniqueConstraint>(), Enumerable.Empty<IDefaultValueConstraint>(), Enumerable.Empty<IForeignKeyConstraint>(), Enumerable.Empty<IIndex>(), Enumerable.Empty<ICheckConstraint>(), Enumerable.Empty<IMetadata>()) });
            sut.AddTables(new[] { new Table("Name", "", Enumerable.Empty<ITableField>(), Enumerable.Empty<IPrimaryKeyConstraint>(), Enumerable.Empty<IUniqueConstraint>(), Enumerable.Empty<IDefaultValueConstraint>(), Enumerable.Empty<IForeignKeyConstraint>(), Enumerable.Empty<IIndex>(), Enumerable.Empty<ICheckConstraint>(), Enumerable.Empty<IMetadata>()) }.AsEnumerable());

            // Assert
            sut.Tables.Should().HaveCount(4);
        }
        
        [Fact]
        public void Can_Construct_Builder_From_Entity_Instance()
        {
            // Arrange
            var instance = new SchemaBuilder()
                .WithName("Name")
                .AddStoredProcedures(new StoredProcedureBuilder().WithName("SP"))
                .AddTables(new TableBuilder().WithName("Table").AddFields(new TableFieldBuilder().WithName("Test").WithType(SqlTableFieldTypes.Date)))
                .AddViews(new ViewBuilder().WithName("View"))
                .AddMetadata(new MetadataBuilder().WithName("MName").WithValue("MValue"))
                .Build();

            // Act
            var actual = new SchemaBuilder(instance);

            // Assert
            actual.Name.Should().Be(instance.Name);
            actual.StoredProcedures.Should().ContainSingle();
            actual.StoredProcedures.First().Name.Should().Be(instance.StoredProcedures.First().Name);
            actual.Tables.Should().ContainSingle();
            actual.Tables.First().Name.Should().Be(instance.Tables.First().Name);
            actual.Views.Should().ContainSingle();
            actual.Views.First().Name.Should().Be(instance.Views.First().Name);
            actual.Metadata.Should().ContainSingle();
            actual.Metadata.First().Name.Should().Be(instance.Metadata.First().Name);
            actual.Metadata.First().Value.Should().Be(instance.Metadata.First().Value);
        }
    }
}
