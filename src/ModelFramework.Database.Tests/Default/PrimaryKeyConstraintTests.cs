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
    public class PrimaryKeyConstraintTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new PrimaryKeyConstraint
            (
                "",
                "",
                new[] { new PrimaryKeyConstraintField("name", false, Enumerable.Empty<IMetadata>()) },
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_Fields()
        {
            // Arrange
            var action = new Action(() => _ = new PrimaryKeyConstraint
            (
                "name",
                "",
                Enumerable.Empty<IPrimaryKeyConstraintField>(),
                Enumerable.Empty<IMetadata>()
            ));

            // Act & Assert
            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("fields");
        }
    }
}
