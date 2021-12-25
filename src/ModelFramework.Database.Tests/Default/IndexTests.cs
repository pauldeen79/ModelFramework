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
    public class IndexTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new Database.Default.Index("", true, "", new[] { new IndexField("name", false, Enumerable.Empty<IMetadata>()) }, Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Ctor_Throws_On_Empty_Fields()
        {
            // Arrange
            var action = new Action(() => _ = new Database.Default.Index("name", true, "", Enumerable.Empty<IIndexField>(), Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("fields");
        }
    }
}
