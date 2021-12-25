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
    public class StoredProcedureTests
    {
        [Fact]
        public void Ctor_Throws_On_Empty_Name()
        {
            // Arrange
            var action = new Action(() => _ = new StoredProcedure("", Enumerable.Empty<IStoredProcedureParameter>(), Enumerable.Empty<ISqlStatement>(), Enumerable.Empty<IMetadata>()));

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("name");
        }
    }
}
