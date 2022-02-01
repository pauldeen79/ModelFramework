namespace ModelFramework.Objects.Tests;

public class ClassPropertyTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new ClassPropertyBuilder().WithTypeName("System.String").Build());

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Fact]
    public void Ctor_Throws_On_Empty_TypeName()
    {
        // Arrange
        var action = new Action(() => _ = new ClassPropertyBuilder().WithName("Test").Build());

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("TypeName cannot be null or whitespace");
    }

    [Fact]
    public void ToString_Returns_Name()
    {
        // Arrange
        var sut = new ClassPropertyBuilder().WithName("Test").WithTypeName("System.String").Build();

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be("Test");
    }
}
