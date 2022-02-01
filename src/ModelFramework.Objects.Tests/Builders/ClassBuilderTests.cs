namespace ModelFramework.Objects.Tests.Builders;

public class ClassBuilderTests
{
    [Fact]
    public void Can_Specify_Baseclass_Using_Type()
    {
        // Arrange
        var sut = new ClassBuilder();

        // Act
        var actual = sut.WithBaseClass(GetType());

        // Assert
        actual.BaseClass.Should().Be(GetType().FullName);
    }

    [Fact]
    public void Can_Add_Usings_For_CodeGeneration()
    {
        // Arrange
        var sut = new ClassBuilder();

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
