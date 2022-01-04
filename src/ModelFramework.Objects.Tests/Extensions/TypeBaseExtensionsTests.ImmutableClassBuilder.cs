using System;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
using Xunit;

namespace ModelFramework.Objects.Tests.Extensions
{
    public partial class TypeBaseExtensionsTests
    {
        [Fact]
        public void Generating_ImmutableClass_From_Class_Without_Properties_Throws_Exception()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToImmutableClass(new ImmutableClassSettings()))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("To create an immutable class, there must be at least one property");
        }
    }
}
