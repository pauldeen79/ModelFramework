using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;
using ModelFramework.Database.Default;
using Xunit;

namespace ModelFramework.Database.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class CheckConstraintTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new CheckConstraint("", "expression", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_Expression()
        {
            // Arrange
            var action = new Action(() => _ = new CheckConstraint("name", "", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("expression");
        }

        [Fact]
        public void Can_Create_CheckConstraint()
        {
            // Act
            var sut = new CheckConstraint("name", "expression", new[] { new Metadata("value", "name") });

            // Assert
            sut.Name.Should().Be("name");
            sut.Expression.Should().Be("expression");
            sut.Metadata.Should().ContainSingle();
            sut.Metadata.First().Name.Should().Be("name");
            sut.Metadata.First().Value.Should().Be("value");
        }
    }
}
