﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using Xunit;

namespace ModelFramework.Database.Tests
{
    [ExcludeFromCodeCoverage]
    public class IndexTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new Index(new[] { new IndexField(false, "name", Enumerable.Empty<IMetadata>()) },
                                                        true,
                                                        string.Empty,
                                                        Enumerable.Empty<IMetadata>(),
                                                        string.Empty));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_Fields()
        {
            // Arrange
            var action = new Action(() => _ = new Index(Enumerable.Empty<IIndexField>(),
                                                        true,
                                                        "name",
                                                        Enumerable.Empty<IMetadata>(),
                                                        string.Empty));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Fields should contain at least 1 value");
        }

        [Fact]
        public void ToString_Returns_Name()
        {
            // Arrange
            var sut = new Index(new[] { new IndexField(false, "name", Enumerable.Empty<IMetadata>()) },
                                true,
                                "name",
                                Enumerable.Empty<IMetadata>(),
                                string.Empty);

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be(sut.Name);
        }
    }
}