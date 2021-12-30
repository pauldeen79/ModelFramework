using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Default;
using ModelFramework.Database.SqlStatements;
using ModelFramework.Database.SqlStatements.Builders;
using Xunit;

namespace ModelFramework.Database.Tests.SqlStatements.Builders
{
    [ExcludeFromCodeCoverage]
    public class LiteralSqlStatementBuilderTests
    {
        [Fact]
        public void Can_Create_Instance_From_Existing_Entity_Instance()
        {
            // Arrange
            var input = new LiteralSqlStatement("--Statement goes here", new[] { new Metadata("Name", "Value") });

            // Act
            var sut = new LiteralSqlStatementBuilder(input);

            // Assert
            sut.Statement.Should().Be(input.Statement);
            sut.Metadata.Should().ContainSingle();
            sut.Metadata.First().Name.Should().Be(input.Metadata.First().Name);
            sut.Metadata.First().Value.Should().Be(input.Metadata.First().Value);
        }

        [Fact]
        public void Can_Build_Entity_Instance_With_All_Properties()
        {
            // Arrange
            var sut = new LiteralSqlStatementBuilder();

            // Act
            var actual = sut.WithStatement("--Statement goes here")
                            .AddMetadata(new MetadataBuilder().WithName("Name").WithValue("Value"))
                            .Build() as LiteralSqlStatement;

            // Assert
            actual.Should().NotBeNull();
            if (actual != null)
            {
                actual.Statement.Should().Be("--Statement goes here");
                actual.Metadata.Should().ContainSingle();
                actual.Metadata.First().Name.Should().Be("Name");
                actual.Metadata.First().Value.Should().Be("Value");
            }
        }
    }
}
