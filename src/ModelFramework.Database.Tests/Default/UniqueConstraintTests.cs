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
    public class UniqueConstraintTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new UniqueConstraint(new[] { new UniqueConstraintField("name", Enumerable.Empty<IMetadata>()) },
                                                                   "",
                                                                   Enumerable.Empty<IMetadata>(),
                                                                   ""));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_Fields()
        {
            // Arrange
            var action = new Action(() => _ = new UniqueConstraint(Enumerable.Empty<IUniqueConstraintField>(),
                                                                   "name",
                                                                   Enumerable.Empty<IMetadata>(),
                                                                   ""));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Fields should contain at least 1 value");
        }
    }
}
