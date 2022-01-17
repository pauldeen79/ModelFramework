using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Database.Builders;
using Xunit;

namespace ModelFramework.Database.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class TableFieldTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new TableFieldBuilder().WithType("int").Build());

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_Type()
        {
            // Arrange
            var action = new Action(() => _ = new TableFieldBuilder().WithName("test").Build());

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Type cannot be null or whitespace");
        }
    }
}
