﻿using System;
using System.ComponentModel.DataAnnotations;
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
    public class DefaultValueConstraintTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new DefaultValueConstraint("fieldName", "defaultValue", "", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_FieldName()
        {
            // Arrange
            var action = new Action(() => _ = new DefaultValueConstraint("", "defaultValue", "name", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("FieldName cannot be null or whitespace");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_DefaultValue()
        {
            // Arrange
            var action = new Action(() => _ = new DefaultValueConstraint("fieldName", "", "name", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ValidationException>().WithMessage("DefaultValue cannot be null or whitespace");
        }

        [Fact]
        public void Can_Create_DefaultValueConstraint()
        {
            // Act
            var sut = new DefaultValueConstraint("fieldName", "defaultValue", "name1", new[] { new Metadata("value", "name2") });

            // Asert
            sut.FieldName.Should().Be("fieldName");
            sut.DefaultValue.Should().Be("defaultValue");
            sut.Name.Should().Be("name1");
            sut.Metadata.Should().ContainSingle();
            sut.Metadata.First().Name.Should().Be("name2");
            sut.Metadata.First().Value.Should().Be("value");
        }
    }
}
