namespace ModelFramework.Database.Tests;

public class UniqueConstraintFieldTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new UniqueConstraintField("", Enumerable.Empty<IMetadata>()));

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Fact]
    public void ToString_Returns_Name()
    {
        // Arrange
        var sut = new UniqueConstraintField("test", Enumerable.Empty<IMetadata>());

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(sut.Name);
    }
}
