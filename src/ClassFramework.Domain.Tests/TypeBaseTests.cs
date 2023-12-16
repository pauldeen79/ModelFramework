namespace ClassFramework.Domain.Tests;

public class TypeBaseTests
{
    public class GetFullName
    {
        [Fact]
        public void Returns_Full_Name_When_Namespace_Is_Present()
        {
            // Arrange
            var sut = new Class
            (
                default,
                default,
                default,
                Enumerable.Empty<Class>(),
                Enumerable.Empty<Enumeration>(),
                "MyNamespace",
                default,
                Enumerable.Empty<string>(),
                Enumerable.Empty<Field>(),
                Enumerable.Empty<Property>(),
                Enumerable.Empty<Method>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<Metadata>(),
                default,
                "MyClass",
                Enumerable.Empty<Attribute>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<Constructor>(),
                default,
                default
            );

            // Act
            var result = sut.GetFullName();

            // Assert
            result.Should().Be("MyNamespace.MyClass");
        }

        [Fact]
        public void Returns_Name_When_Namespace_Is_Not_Present()
        {
            // Arrange
            var sut = new Class
            (
                default,
                default,
                default,
                Enumerable.Empty<Class>(),
                Enumerable.Empty<Enumeration>(),
                string.Empty,
                default,
                Enumerable.Empty<string>(),
                Enumerable.Empty<Field>(),
                Enumerable.Empty<Property>(),
                Enumerable.Empty<Method>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<Metadata>(),
                default,
                "MyClass",
                Enumerable.Empty<Attribute>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<Constructor>(),
                default,
                default
            );

            // Act
            var result = sut.GetFullName();

            // Assert
            result.Should().Be("MyClass");
        }
    }
}
