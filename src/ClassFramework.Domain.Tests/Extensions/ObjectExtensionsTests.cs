namespace ClassFramework.Domain.Tests.Extensions;

public class ObjectExtensionsTests
{
    [Fact]
    public void CsharpFormat_Returns_Formatted_String_Value()
    {
        // Arrange
        var input = "test";

        // Act
        var actual = input.CsharpFormat(CultureInfo.InvariantCulture);

        // Assert
        actual.Should().Be(@"@""test""");
    }

    [Fact]
    public void CsharpFormat_Returns_Boolean_String_False()
    {
        // Arrange
        var input = false;

        // Act
        var actual = input.CsharpFormat(CultureInfo.InvariantCulture);

        // Assert
        actual.Should().Be(@"false");
    }

    [Fact]
    public void CsharpFormat_Returns_Boolean_String_True()
    {
        // Arrange
        var input = true;

        // Act
        var actual = input.CsharpFormat(CultureInfo.InvariantCulture);

        // Assert
        actual.Should().Be(@"true");
    }

    [Fact]
    public void CsharpFormat_Returns_Null_String()
    {
        // Arrange
        var input = default(object?);

        // Act
        var actual = input.CsharpFormat(CultureInfo.InvariantCulture);

        // Assert
        actual.Should().Be(@"null");
    }

    [Fact]
    public void CsharpFormat_Returns_Int32_String()
    {
        // Arrange
        var input = 12345;

        // Act
        var actual = input.CsharpFormat(CultureInfo.InvariantCulture);

        // Assert
        actual.Should().Be(@"12345");
    }

    [Fact]
    public void CsharpFormat_Returns_Decimal_String_With_Custom_CultureInfo()
    {
        // Arrange
        var input = 123.45;

        // Act
        var actual = input.CsharpFormat(new CultureInfo("nl-NL"));

        // Assert
        actual.Should().Be(@"123,45");
    }

    [Fact]
    public void CsharpFormat_Returns_String_For_Custom_Type()
    {
        // Arrange
        var input = new { };

        // Act
        var actual = input.CsharpFormat(CultureInfo.InvariantCulture);

        // Assert
        actual.Should().Be(@"{ }");
    }
}
