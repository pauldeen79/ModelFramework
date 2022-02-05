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
}
