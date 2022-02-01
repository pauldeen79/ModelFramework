namespace ModelFramework.Objects.Tests;

public class AttributeParameterTests
{
    [Fact]
    public void ToString_Returns_Name_When_Name_Is_Not_Empty()
    {
        // Arrange
        var sut = new AttributeParameter("TestValue", Enumerable.Empty<IMetadata>(), "TestName");

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be("TestName");
    }

    [Fact]
    public void ToString_Returns_Value_When_Name_Is_Empty()
    {
        // Arrange
        var sut = new AttributeParameter("TestValue", Enumerable.Empty<IMetadata>(), string.Empty);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be("TestValue");
    }

    [Fact]
    public void ToString_Returns_String_Empty_When_Name_And_Value_Are_Both_Empty()
    {
        // Arrange
        var sut = new AttributeParameter(string.Empty, Enumerable.Empty<IMetadata>(), string.Empty);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().BeEmpty();
    }
}
