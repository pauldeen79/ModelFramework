namespace ModelFramework.Database.Tests;

public class TableFieldTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new TableFieldBuilder().WithType("INT").Build());

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Fact]
    public void Ctor_Throws_On_Empty_Type()
    {
        // Arrange
        var action = new Action(() => _ = new TableFieldBuilder().WithName("test").Build());

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Type cannot be null or whitespace");
    }

    [Fact]
    public void ToString_Returns_Name()
    {
        // Arrange
        var sut = new TableFieldBuilder().WithName("test").WithType("INT").Build();

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(sut.Name);
    }
}
