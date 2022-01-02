using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Extensions;
using Xunit;

namespace ModelFramework.Objects.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ClassExtensionsTests
    {
        [Fact]
        public void IsPoco_Returns_True_On_Class_With_Public_Parameterless_Contructor_And_Writable_Properties()
        {
            // Assert
            var input = new ClassBuilder().WithName("Test")
                                          .AddProperties(new ClassPropertyBuilder().WithName("Name")
                                                                                   .WithType(typeof(string))
                                                                                   .AsReadOnly())
                                          .AddConstructors(new ClassConstructorBuilder().AddParameter("name", typeof(string))
                                                                                        .AddLiteralCodeStatements("Name = name;"))
                                          .Build();

            // Act
            var actual = input.IsPoco();

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void IsPoco_Returns_False_On_Class_With_Public_Contructor_With_Parameters_And_No_Writable_Properties()
        {
            // Assert
            var input = new ClassBuilder().WithName("Test")
                                          .AddProperties(new ClassPropertyBuilder().WithName("Name")
                                                                                   .WithType(typeof(string)))
                                          .Build();

            // Act
            var actual = input.IsPoco();

            // Assert
            actual.Should().BeTrue();
        }
    }
}
