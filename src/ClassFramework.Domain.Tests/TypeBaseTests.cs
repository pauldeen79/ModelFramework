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
                Enumerable.Empty<ClassProperty>(),
                Enumerable.Empty<ClassMethod>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<Metadata>(),
                default,
                "MyClass",
                Enumerable.Empty<Attribute>(),
                Enumerable.Empty<ClassField>(),
                Enumerable.Empty<ClassConstructor>(),
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
                Enumerable.Empty<ClassProperty>(),
                Enumerable.Empty<ClassMethod>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<Metadata>(),
                default,
                "MyClass",
                Enumerable.Empty<Attribute>(),
                Enumerable.Empty<ClassField>(),
                Enumerable.Empty<ClassConstructor>(),
                default,
                default
            );

            // Act
            var result = sut.GetFullName();

            // Assert
            result.Should().Be("MyClass");
        }
    }

    public class GetGenericTypeArgumentsString
    {
        [Fact]
        public void Returns_Empty_String_When_No_GenericArguments_Are_Present()
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
                Enumerable.Empty<ClassProperty>(),
                Enumerable.Empty<ClassMethod>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<Metadata>(),
                default,
                "MyClass",
                Enumerable.Empty<Attribute>(),
                Enumerable.Empty<ClassField>(),
                Enumerable.Empty<ClassConstructor>(),
                default,
                default
            );

            // Act
            var result = sut.GetGenericTypeArgumentsString();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_When_GenericArguments_Are_Present()
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
                Enumerable.Empty<ClassProperty>(),
                Enumerable.Empty<ClassMethod>(),
                [ "T" ],
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                Enumerable.Empty<Metadata>(),
                default,
                "MyClass",
                Enumerable.Empty<Attribute>(),
                Enumerable.Empty<ClassField>(),
                Enumerable.Empty<ClassConstructor>(),
                default,
                default
            );

            // Act
            var result = sut.GetGenericTypeArgumentsString();

            // Assert
            result.Should().Be("<T>");
        }
    }
}
