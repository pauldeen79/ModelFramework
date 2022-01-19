using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using Xunit;

namespace ModelFramework.Objects.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class InterfaceBuilderTests
    {
        [Fact]
        public void Can_Add_Interface_Using_Type()
        {
            // Arrange
            var sut = new InterfaceBuilder();

            // Act
            var actual = sut.AddInterfaces(typeof(INotifyPropertyChanged));

            // Assert
            actual.Interfaces.Should().BeEquivalentTo(new[] { typeof(INotifyPropertyChanged).FullName });
        }

        [Fact]
        public void Can_Add_Usings_For_CodeGeneration()
        {
            // Arrange
            var sut = new InterfaceBuilder();

            // Act
            var actual = sut.AddUsings("ModelFramework.Objects.Contracts", "ModelFramework.Database.Contracts");

            // Assert
            actual.Metadata.Select(x => $"{x.Name} = {x.Value}").Should().BeEquivalentTo(new[]
            {
                $"{MetadataNames.CustomUsing} = ModelFramework.Objects.Contracts",
                $"{MetadataNames.CustomUsing} = ModelFramework.Database.Contracts"
            });
        }
    }
}
