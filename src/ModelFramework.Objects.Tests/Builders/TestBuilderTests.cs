namespace ModelFramework.Objects.Tests.Builders;

public class TestBuilderTests
{
    [Fact]
    public void Can_Generate_Default_Entity_From_New_Builder()
    {
        // Arrange
        var sut = new TestBuilder();

        // Act
        var actual = sut.Build();

        // Assert
        actual.Name.Should().BeEmpty();
        actual.Value.Should().BeOfType<object>();
        actual.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void Can_Generate_Fully_Filled_Entity_From_Builder()
    {
        // Arrange
        var sut = new TestBuilder().WithName("test")
                                   .WithValue(18)
                                   .AddMetadata(new MetadataBuilder().WithName("Test"));

        // Act
        var actual = sut.Build();

        // Assert
        actual.Name.Should().Be("test");
        actual.Value.Should().Be(18);
        actual.Metadata.Should().ContainSingle();
        actual.Metadata.First().Name.Should().Be("Test");
    }

    [Fact]
    public void Can_Generate_Lazy_Filled_Entity_From_Builder()
    {
        // Arrange
        var dict = new Dictionary<string, object>();
        var sut = new TestBuilder().WithName(() => (string)dict["Name"])
                                   .WithValue(() => dict["Value"])
                                   .AddMetadata(new MetadataBuilder().WithName("Test"));
        dict["Name"] = "test";
        dict["Value"] = 18;

        // Act
        var actual = sut.Build();

        // Assert
        actual.Name.Should().Be("test");
        actual.Value.Should().Be(18);
        actual.Metadata.Should().ContainSingle();
        actual.Metadata.First().Name.Should().Be("Test");
    }
}
