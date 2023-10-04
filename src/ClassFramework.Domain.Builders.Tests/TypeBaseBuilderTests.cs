namespace ClassFramework.Domain.Builders.Tests;

public class TypeBaseBuilderTests
{
    public class GetFullName
    {
        [Fact]
        public void Returns_Full_Name_When_Namespace_Is_Present()
        {
            // Arrange
            var sut = new ClassBuilder().WithNamespace("MyNamespace").WithName("MyClass");

            // Act
            var result = sut.GetFullName();

            // Assert
            result.Should().Be("MyNamespace.MyClass");
        }

        [Fact]
        public void Returns_Name_When_Namespace_Is_Not_Present()
        {
            // Arrange
            var sut = new ClassBuilder().WithNamespace(string.Empty).WithName("MyClass");

            // Act
            var result = sut.GetFullName();

            // Assert
            result.Should().Be("MyClass");
        }
    }
}
