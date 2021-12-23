using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Common.Default;
using Xunit;

namespace ModelFramework.Common.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class MetadataTests
    {
        [Fact]
        public void CtorThrowsOnNullName()
        {
            // Arrange
            var action = new Action(() => _ = new Metadata(string.Empty, "Value"));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }
    }
}
