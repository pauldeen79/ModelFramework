namespace ModelFramework.Common.Tests.Extensions;

public class ObjectExtensionsTests
{
    [Fact]
    public void CsharpFormat_Returns_Formatted_String_Value()
    {
        // Arrange
        var input = "test";

        // Act
        var actual = input.CsharpFormat();

        // Assert
        actual.Should().Be(@"@""test""");
    }

    [Fact]
    public void CsharpFormat_Returns_Boolean_String()
    {
        // Arrange
        var input = false;

        // Act
        var actual = input.CsharpFormat();

        // Assert
        actual.Should().Be(@"false");
    }

    [Fact]
    public void CsharpFormat_Returns_Int32_String()
    {
        // Arrange
        var input = 12345;

        // Act
        var actual = input.CsharpFormat();

        // Assert
        actual.Should().Be(@"12345");
    }
}
