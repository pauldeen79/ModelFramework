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
    public class UniqueConstraintTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new UniqueConstraint("", "", new[] { new UniqueConstraintField("name", Enumerable.Empty<IMetadata>()) }, Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_Fields()
        {
            // Arrange
            var action = new Action(() => _ = new UniqueConstraint("name", "", Enumerable.Empty<IUniqueConstraintField>(), Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("fields");
        }
    }
}
