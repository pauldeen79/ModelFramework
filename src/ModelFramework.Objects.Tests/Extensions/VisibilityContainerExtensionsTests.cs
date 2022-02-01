namespace ModelFramework.Objects.Tests.Extensions;

public class VisibilityContainerExtensionsTests
{
    [Fact]
    public void Can_Specify_Custom_Modifiers()
    {
        // Arrange
        var sut = new ClassFieldBuilder().WithName("Test")
                                         .WithTypeName("System.Int32")
                                         .WithCustomModifiers("internal readonly")
                                         .Build();

        // Act
        var actual = sut.GetModifiers();

        // Assert
        actual.Should().Be("internal readonly ");
    }

    [Fact]
    public void Can_Specify_Constant_On_Field()
    {
        // Arrange
        var sut = new ClassFieldBuilder().WithName("Test")
                                         .WithTypeName("System.Int32")
                                         .WithConstant()
                                         .Build();

        // Act
        var actual = sut.GetModifiers();

        // Assert
        actual.Should().Be("private const ");
    }
}
