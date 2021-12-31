using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using Xunit;

namespace ModelFramework.Database.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class ForeignKeyConstraintTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new ForeignKeyConstraint
            (
                "",
                "foreignTableName",
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                CascadeAction.NoAction,
                CascadeAction.NoAction,
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_ForeignTableName()
        {
            // Arrange
            var action = new Action(() => _ = new ForeignKeyConstraint
            (
                "name",
                "",
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                CascadeAction.NoAction,
                CascadeAction.NoAction,
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("foreignTableName");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_LocalFields()
        {
            // Arrange
            var action = new Action(() => _ = new ForeignKeyConstraint
            (
                "name",
                "foreignTableName",
                Enumerable.Empty<IForeignKeyConstraintField>(),
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                CascadeAction.NoAction,
                CascadeAction.NoAction,
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("localFields");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_ForeignFields()
        {
            // Arrange
            var action = new Action(() => _ = new ForeignKeyConstraint
            (
                "name",
                "foreignTableName",
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                Enumerable.Empty<IForeignKeyConstraintField>(),
                CascadeAction.NoAction,
                CascadeAction.NoAction,
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("foreignFields");
        }
    }
}
