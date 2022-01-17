using System;
using System.ComponentModel.DataAnnotations;
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
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                "foreignTableName",
                CascadeAction.NoAction,
                CascadeAction.NoAction,
                "",
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_ForeignTableName()
        {
            // Arrange
            var action = new Action(() => _ = new ForeignKeyConstraint
            (
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                "",
                CascadeAction.NoAction,
                CascadeAction.NoAction,
                "name",
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("ForeignTableName cannot be null or whitespace");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_LocalFields()
        {
            // Arrange
            var action = new Action(() => _ = new ForeignKeyConstraint
            (
                Enumerable.Empty<IForeignKeyConstraintField>(),
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                "foreignTableName",
                CascadeAction.NoAction,
                CascadeAction.NoAction,
                "name",
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("LocalFields should contain at least 1 value");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_ForeignFields()
        {
            // Arrange
            var action = new Action(() => _ = new ForeignKeyConstraint
            (
                new[] { new ForeignKeyConstraintField("name", Enumerable.Empty<IMetadata>()) },
                Enumerable.Empty<IForeignKeyConstraintField>(),
                "foreignTableName",
                CascadeAction.NoAction,
                CascadeAction.NoAction,
                "name",
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("ForeignFields should contain at least 1 value");
        }
    }
}
