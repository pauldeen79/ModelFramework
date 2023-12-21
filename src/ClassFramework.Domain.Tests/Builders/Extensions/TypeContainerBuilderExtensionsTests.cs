namespace ClassFramework.Domain.Tests.Builders.Extensions;

public class TypeContainerBuilderExtensionsTests : TestBase<PropertyBuilder>
{
    public class WithType_Type : TypeContainerBuilderExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.WithType(type: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("type");
        }

        [Fact]
        public void Adds_Correct_Information()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.WithType(typeof(int));

            // Assert
            result.TypeName.Should().Be("System.Int32");
        }
    }

    public class WithType_TypeBase : TypeContainerBuilderExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.WithType(typeBase: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("typeBase");
        }

        [Fact]
        public void Adds_Correct_Information_For_Class()
        {
            // Arrange
            var sut = CreateSut();
            var typeBase = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act
            var result = sut.WithType(typeBase);

            // Assert
            result.TypeName.Should().Be("MyNamespace.MyClass");
            result.IsValueType.Should().BeFalse();
        }

        [Fact]
        public void Adds_Correct_Information_For_Struct()
        {
            // Arrange
            var sut = CreateSut();
            var typeBase = new StructBuilder().WithName("MyStruct").WithNamespace("MyNamespace").Build();

            // Act
            var result = sut.WithType(typeBase);

            // Assert
            result.TypeName.Should().Be("MyNamespace.MyStruct");
            result.IsValueType.Should().BeTrue();
        }
    }
}
