namespace ClassFramework.Domain.Tests;

public class TypeBaseTests
{
    public class GetFullName
    {
        [Fact]
        public void Returns_Full_Name_When_Namespace_Is_Present()
        {
            // Arrange
            var sut = new Class(Enumerable.Empty<ClassField>(), default, default, default, Enumerable.Empty<Class>(), Enumerable.Empty<ClassConstructor>(), Enumerable.Empty<Enumeration>(), default, default, "MyNamespace", default, Enumerable.Empty<string>(), Enumerable.Empty<ClassProperty>(), Enumerable.Empty<ClassMethod>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<Metadata>(), default, "MyClass", Enumerable.Empty<Attribute>());

            // Act
            var result = sut.GetFullName();

            // Assert
            result.Should().Be("MyNamespace.MyClass");
        }

        [Fact]
        public void Returns_Name_When_Namespace_Is_Not_Present()
        {
            // Arrange
            var sut = new Class(Enumerable.Empty<ClassField>(), default, default, default, Enumerable.Empty<Class>(), Enumerable.Empty<ClassConstructor>(), Enumerable.Empty<Enumeration>(), default, default, string.Empty, default, Enumerable.Empty<string>(), Enumerable.Empty<ClassProperty>(), Enumerable.Empty<ClassMethod>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<Metadata>(), default, "MyClass", Enumerable.Empty<Attribute>());

            // Act
            var result = sut.GetFullName();

            // Assert
            result.Should().Be("MyClass");
        }
    }
}
