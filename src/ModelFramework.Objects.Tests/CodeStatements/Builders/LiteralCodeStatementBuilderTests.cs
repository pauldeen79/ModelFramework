using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Default;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.CodeStatements.Builders;
using Xunit;

namespace ModelFramework.Objects.Tests.CodeStatements.Builders
{
    [ExcludeFromCodeCoverage]
    public class LiteralCodeStatementBuilderTests
    {
        [Fact]
        public void Can_Create_Instance_From_Existing_Entity_Instance()
        {
            // Arrange
            var input = new LiteralCodeStatement("//Statement goes here", new[] { new Metadata("Name", "Value") });

            // Act
            var sut = new LiteralCodeStatementBuilder(input);

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
            var sut = new LiteralCodeStatementBuilder();

            // Act
            var actual = sut.WithStatement("//Statement goes here")
                            .AddMetadata(new MetadataBuilder().WithName("Name").WithValue("Value"))
                            .Build() as LiteralCodeStatement;

            // Assert
            actual.Should().NotBeNull();
            if (actual != null)
            {
                actual.Statement.Should().Be("//Statement goes here");
                actual.Metadata.Should().ContainSingle();
                actual.Metadata.First().Name.Should().Be("Name");
                actual.Metadata.First().Value.Should().Be("Value");
            }
        }
    }
}
