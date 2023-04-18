namespace ModelFramework.Objects.Tests.Builders;

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
        actual.Interfaces.Should().BeEquivalentTo(typeof(INotifyPropertyChanged).FullName);
    }

    [Fact]
    public void Can_Add_Usings_For_CodeGeneration()
    {
        // Arrange
        var sut = new InterfaceBuilder();

        // Act
        var actual = sut.AddUsings("ModelFramework.Objects.Contracts", "ModelFramework.Database.Contracts");

        // Assert
        actual.Metadata.Select(x => $"{x.Name} = {x.Value}").Should().BeEquivalentTo(
            $"{MetadataNames.CustomUsing} = ModelFramework.Objects.Contracts",
            $"{MetadataNames.CustomUsing} = ModelFramework.Database.Contracts"
        );
    }

    [Fact]
    public void Can_Add_Abbreviated_Namespaces_For_CodeGeneration()
    {
        // Arrange
        var sut = new InterfaceBuilder();

        // Act
        var actual = sut.AddNamespacesToAbbreviate("ModelFramework.Objects.Contracts", "ModelFramework.Database.Contracts");

        // Assert
        actual.Metadata.Select(x => $"{x.Name} = {x.Value}").Should().BeEquivalentTo(
            $"{MetadataNames.NamespaceToAbbreviate} = ModelFramework.Objects.Contracts",
            $"{MetadataNames.NamespaceToAbbreviate} = ModelFramework.Database.Contracts"
        );
    }
}
