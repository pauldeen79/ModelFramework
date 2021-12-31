using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Extensions;
using Xunit;

namespace ModelFramework.Database.Tests
{
    [ExcludeFromCodeCoverage]
    public class CascadeActionExtensionsTests
    {
        [Theory]
        [InlineData(CascadeAction.NoAction, "NO ACTION")]
        [InlineData(CascadeAction.Cascade, "CASCADE")]
        [InlineData(CascadeAction.SetNull, "SET NULL")]
        [InlineData(CascadeAction.SetDefault, "SET DEFAULT")]
        public void ToSql_Returns_Correct_Result(CascadeAction input, string expectedResult)
        {
            // Act
            var actual = input.ToSql();

            // Assert
            actual.Should().Be(expectedResult);
        }
    }
}
