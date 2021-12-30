﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Default;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.CodeStatements.Builders;
using Xunit;

namespace ModelFramework.Objects.Tests.CodeStatements
{
    [ExcludeFromCodeCoverage]
    public class LiteralCodeStatementTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Statement()
        {
            // Arrange
            var action = new Action(() => _ = new LiteralCodeStatement(string.Empty, Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("statement");
        }

        [Fact]
        public void CreateBuilder_Creates_Builder()
        {
            // Arrange
            var sut = new LiteralCodeStatement("//Statement goes here", new[] { new Metadata("Name", "Value") });

            // Act
            var actual = sut.CreateBuilder() as LiteralCodeStatementBuilder;

            // Asset
            actual.Should().NotBeNull();
            if (actual != null)
            {
                actual.Statement.Should().Be(sut.Statement);
                actual.Metadata.Should().ContainSingle();
                actual.Metadata.First().Name.Should().Be(sut.Metadata.First().Name);
                actual.Metadata.First().Value.Should().Be(sut.Metadata.First().Value);
            }
        }

        [Fact]
        public void ToString_Returns_Statement()
        {
            // Arrange
            var sut = new LiteralCodeStatement("//Statement goes here", Enumerable.Empty<IMetadata>());

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be(sut.Statement);
        }
    }
}
