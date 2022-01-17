using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Default;
using Xunit;

namespace ModelFramework.Database.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class ForeignKeyConstraintFieldTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new ForeignKeyConstraintField("", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }
    }
}
