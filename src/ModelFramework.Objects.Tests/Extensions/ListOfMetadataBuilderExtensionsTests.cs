namespace ModelFramework.Objects.Tests.Extensions;

public class ListOfMetadataBuilderExtensionsTests
{
    [Fact]
    public void Can_Replace_Metadata_By_Name()
    {
        // Arrange
        var sut = new List<MetadataBuilder>
        {
            new MetadataBuilder().WithName("Test").WithValue("Old value")
        };

        // Act
        var actual = sut.Replace("Test", "New value");

        // Assert
        actual.Should().ContainSingle();
        actual[0].Name.Should().Be("Test");
        actual[0].Value.Should().Be("New value");
    }

    [Fact]
    public void When_Replacing_Metadata_Other_Metadata_Stays_There()
    {
        // Arrange
        var sut = new List<MetadataBuilder>
        {
            new MetadataBuilder().WithName("Test1").WithValue("Old value")
        };

        // Act
        var actual = sut.Replace("Test2", "New value");

        // Assert
        actual.Should().HaveCount(2);
        actual[0].Name.Should().Be("Test1");
        actual[0].Value.Should().Be("Old value");
        actual[1].Name.Should().Be("Test2");
        actual[1].Value.Should().Be("New value");
    }
}
