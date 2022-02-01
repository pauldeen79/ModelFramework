namespace ModelFramework.Database.Tests;

public class ViewConditionTests
{
    [Fact]
    public void ToString_Returns_Expression_When_Combination_Is_Empty()
    {
        // Arrange
        var sut = new ViewCondition("Expression", string.Empty, Enumerable.Empty<IMetadata>(), string.Empty);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(sut.Expression);
    }

    [Fact]
    public void ToString_Returns_Combination_And_Expression_When_Combination_Is_Not_Empty()
    {
        // Arrange
        var sut = new ViewCondition("Expression", "AND", Enumerable.Empty<IMetadata>(), string.Empty);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be("AND Expression");
    }
}
