using System;
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
        public void Ctor_Throws_On_Null_Name()
        {
            // Arrange
            var action = new Action(() => _ = new DefaultValueConstraint("fieldName", "defaultValue", "", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Ctor_Throws_On_Null_FieldName()
        {
            // Arrange
            var action = new Action(() => _ = new DefaultValueConstraint("", "defaultValue", "name", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("fieldName");
        }

        [Fact]
        public void Ctor_Throws_On_Null_DefaultValue()
        {
            // Arrange
            var action = new Action(() => _ = new DefaultValueConstraint("fieldName", "", "name", Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("defaultValue");
        }

        [Fact]
        public void Can_Create_DefaultValueConstraint()
        {
            // Act
            var sut = new DefaultValueConstraint("fieldName", "defaultValue", "name1", new[] { new Metadata("name2", "value") });

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
