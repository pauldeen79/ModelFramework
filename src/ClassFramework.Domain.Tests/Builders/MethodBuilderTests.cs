namespace ClassFramework.Domain.Tests.Builders;

public class MethodBuilderTests
{
    public class WithReturnType_Type
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Arrange
            var sut = new MethodBuilder();

            // Act & Assert
            sut.Invoking(x => x.WithReturnType(type: default(Type)!))
               .Should().Throw<ArgumentNullException>().WithParameterName("type");
        }

        [Fact]
        public void Fills_Properties_Correctly_On_Non_Null_Type()
        {
            // Arrange
            var sut = new MethodBuilder();

            // Act
            var result = sut.WithReturnType(typeof(MethodBuilderTests));

            // Assert
            result.ReturnTypeName.Should().Be("ClassFramework.Domain.Tests.Builders.MethodBuilderTests");
            result.ReturnTypeIsValueType.Should().BeFalse();
        }
    }

    public class WithReturnType_IType
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Arrange
            var sut = new MethodBuilder();

            // Act & Assert
            sut.Invoking(x => x.WithReturnType(type: default(IType)!))
               .Should().Throw<ArgumentNullException>().WithParameterName("type");
        }

        [Fact]
        public void Fills_Properties_Correctly_On_Non_Null_Type()
        {
            // Arrange
            var sut = new MethodBuilder();

            // Act
            var result = sut.WithReturnType(new StructBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build());

            // Assert
            result.ReturnTypeName.Should().Be("MyNamespace.MyClass");
            result.ReturnTypeIsValueType.Should().BeTrue();
        }
    }
}
