namespace ModelFramework.Objects.Tests;

public class ParameterTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new Parameter(default,
                                                        default,
                                                        default,
                                                        "System.String",
                                                        default,
                                                        default,
                                                        Enumerable.Empty<IAttribute>(),
                                                        Enumerable.Empty<IMetadata>(),
                                                        string.Empty,
                                                        default));

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Fact]
    public void Ctor_Throws_On_Empty_TypeName()
    {
        // Arrange
        var action = new Action(() => _ = new Parameter(default,
                                                        default,
                                                        default,
                                                        string.Empty,
                                                        default,
                                                        default,
                                                        Enumerable.Empty<IAttribute>(),
                                                        Enumerable.Empty<IMetadata>(),
                                                        "Test",
                                                        default));

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("TypeName cannot be null or whitespace");
    }

    [Fact]
    public void ToString_Returns_Name()
    {
        // Arrange
        var sut = new Parameter(default,
                                default,
                                default,
                                "System.String",
                                default,
                                default,
                                Enumerable.Empty<IAttribute>(),
                                Enumerable.Empty<IMetadata>(),
                                "Test",
                                default);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be("Test");
    }
}
