﻿using System;
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
    public class ViewFieldTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new ViewField(string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void ToString_Returns_Name()
        {
            // Arrange
            var sut = new ViewField(string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    "Test",
                                    Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be(sut.Name);
        }
    }
}
