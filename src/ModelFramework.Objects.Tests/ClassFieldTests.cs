namespace ModelFramework.Objects.Tests;

public class ClassFieldTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new ClassFieldBuilder().WithTypeName("System.String").Build());

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Fact]
    public void Ctor_Throws_On_Empty_TypeName()
    {
        // Arrange
        var action = new Action(() => _ = new ClassFieldBuilder().WithName("Test").Build());

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("TypeName cannot be null or whitespace");
    }

    [Fact]
    public void ToString_Returns_Name()
    {
        // Arrange
        var sut = new ClassFieldBuilder().WithTypeName("System.String").WithName("Test").Build();

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be("Test");
    }
}
