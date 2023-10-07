namespace ClassFramework.Pipelines.Tests.Extensions;

public class TypeBaseExtensionsTests
{
    public class FormatInstanceName
    {
        [Fact]
        public void Returns_FullName_When_FormatInstanceTypeNameDelegate_Is_Null()
        {
            // Arrange
            var sut = new ClassBuilder().WithNamespace("MyNamespace").WithName("MyClass").Build();

            // Act
            var result = sut.FormatInstanceName(true, null);

            // Assert
            result.Should().Be("MyNamespace.MyClass");
        }

        [Fact]
        public void Returns_Simplified_TypeName_When_FormatInstanceTypeNameDelegate_Is_Null_And_Type_Is_Csharp_Keyw0rd()
        {
            // Arrange
            var sut = new ClassBuilder().WithNamespace("System").WithName("Int32").Build();

            // Act
            var result = sut.FormatInstanceName(true, null);

            // Assert
            result.Should().Be("int");
        }

        [Fact]
        public void Returns_FullName_When_FormatInstanceTypeNameDelegate_Is_Not_Null_But_Returns_Empty_String()
        {
            // Arrange
            var sut = new ClassBuilder().WithNamespace("MyNamespace").WithName("MyClass").Build();

            // Act
            var result = sut.FormatInstanceName(true, (_, _) => string.Empty);

            // Assert
            result.Should().Be("MyNamespace.MyClass");
        }

        [Fact]
        public void Returns_Result_From_FormatInstanceTypeNameDelegate_When_Not_Null()
        {
            // Arrange
            var sut = new ClassBuilder().WithNamespace("MyNamespace").WithName("MyClass").Build();

            // Act
            var result = sut.FormatInstanceName(true, (_, _) => "MyCustomValue");

            // Assert
            result.Should().Be("MyCustomValue");
        }
    }
}
