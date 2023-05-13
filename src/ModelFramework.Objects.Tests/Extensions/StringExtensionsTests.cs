namespace ModelFramework.Objects.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void AbbreviateNamespaces_Returns_Value_Unchanged_When_Namespace_Is_Not_Supplied()
    {
        // Arrange
        var sut = "MyNamespace.MyClass";
        var namespacesToAbbreviate = new[] { "System" };

        // Act
        var actual = sut.AbbreviateNamespaces(namespacesToAbbreviate);

        // Assert
        actual.Should().Be(sut);
    }

    [Fact]
    public void AbbreviateNamespaces_Returns_Abbreviated_Value_When_Namespace_Is_Supplied()
    {
        // Arrange
        var sut = "MyNamespace.MyClass";
        var namespacesToAbbreviate = new[] { "MyNamespace" };

        // Act
        var actual = sut.AbbreviateNamespaces(namespacesToAbbreviate);

        // Assert
        actual.Should().Be("MyClass");
    }

    [Theory,
        InlineData("System.String", true, false, "default(System.String?)"),
        InlineData("System.String", false, false, "string.Empty"),
        InlineData("string", true, false, "default(string?)"),
        InlineData("string?", true, false, "default(string?)"),
        InlineData("string", false, false, "string.Empty"),
        InlineData("System.Object", true, false, "default(System.Object?)"),
        InlineData("System.Object", false, false, "new System.Object()"),
        InlineData("object", true, false, "default(object?)"),
        InlineData("object?", true, false, "default(object?)"),
        InlineData("object", false, false, "new System.Object()"),
        InlineData("System.Int32", false, false, "default(System.Int32)"),
        InlineData("System.Int32", true, false, "default(System.Int32?)"),
        InlineData("System.Collections.IEnumerable", false, false, "System.Linq.Enumerable.Empty<object>()"),
        InlineData("System.Collections.IEnumerable", true, false, "default(System.Collections.IEnumerable?)"),
        InlineData("System.Collections.Generic.IEnumerable<int>", false, false, "System.Linq.Enumerable.Empty<int>()"),
        InlineData("System.Collections.Generic.IEnumerable<int>", true, false, "default(System.Collections.Generic.IEnumerable<int>?)"),
        InlineData("SomeType", false, true, "default(SomeType)!")]
    public void GetDefaultValue_Returns_Correct_Result(string input, bool isNullable, bool enableNullableReferenceTypes, string expected)
    {
        // Act
        var actual = input.GetDefaultValue(isNullable, enableNullableReferenceTypes);

        // Assert
        actual.Should().Be(expected);
    }
}
