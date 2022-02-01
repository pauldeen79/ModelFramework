namespace ModelFramework.Objects.Tests;

public class InterfaceTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new InterfaceBuilder().Build());

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Theory]
    [InlineData("Test", "", "Test")]
    [InlineData("Test", "MyNamespace", "MyNamespace.Test")]
    public void ToString_Returns_Correct_Result(string name, string @namespace, string expectedResult)
    {
        // Arrange
        var sut = new InterfaceBuilder().WithName(name).WithNamespace(@namespace).Build();

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(expectedResult);
    }
}
