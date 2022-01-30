using System.Linq;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Extensions;
using Xunit;

namespace ModelFramework.Objects.Tests.Extensions
{
    public partial class TypeBaseExtensionsTests
    {
        [Fact]
        public void Can_Build_Interface_From_Class()
        {
            // Arrange
            var input = CreateClass();

            // Act
            var actual = input.ToInterface();

            // Assert
            actual.Name.Should().Be(input.Name);
            actual.Namespace.Should().Be(input.Namespace);
            actual.Properties.Should().BeEquivalentTo(input.Properties);
            actual.Methods.Should().BeEquivalentTo(input.Methods);
            actual.Interfaces.Should().BeEquivalentTo(input.Interfaces);
            actual.Metadata.Should().BeEquivalentTo(input.Metadata);
            actual.Attributes.Should().BeEquivalentTo(input.Attributes);
            actual.Partial.Should().Be(input.Partial);
            actual.Visibility.Should().Be(input.Visibility);
        }

        [Fact]
        public void Can_Build_InterfaceBuilder_From_Class()
        {
            // Arrange
            var input = CreateClass();

            // Act
            var actual = input.ToInterfaceBuilder().WithName($"I{input.Name}");

            // Assert
            actual.Name.Should().Be($"I{input.Name}");
            actual.Namespace.Should().Be(input.Namespace);
            actual.Properties.Select(x => x.Build()).Should().BeEquivalentTo(input.Properties);
            actual.Methods.Select(x => x.Build()).Should().BeEquivalentTo(input.Methods);
            actual.Interfaces.Should().BeEquivalentTo(input.Interfaces);
            actual.Metadata.Select(x => x.Build()).Should().BeEquivalentTo(input.Metadata);
            actual.Attributes.Select(x => x.Build()).Should().BeEquivalentTo(input.Attributes);
            actual.Partial.Should().Be(input.Partial);
            actual.Visibility.Should().Be(input.Visibility);
        }

        private static IClass CreateClass()
            => new ClassBuilder()
                .WithName("Test")
                .WithNamespace("MyNamespace")
                .AddProperties(new ClassPropertyBuilder().WithName("Property").WithType(typeof(int)))
                .AddMethods(new ClassMethodBuilder().WithName("MyMethod"))
                .AddInterfaces("IMyInterface")
                .AddMetadata("Name", "Value")
                .AddAttributes(new AttributeBuilder().AddNameAndParameter("ReadOnly", true))
                .WithPartial(true)
                .WithVisibility(Visibility.Internal)
                .Build();
    }
}
