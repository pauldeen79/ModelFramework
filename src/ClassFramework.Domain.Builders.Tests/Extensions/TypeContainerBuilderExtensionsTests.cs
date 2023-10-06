namespace ClassFramework.Domain.Builders.Tests.Extensions;

public class TypeContainerBuilderExtensionsTests
{
    public class WithType
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Arrange
            var sut = new ClassMethodBuilder();

            // Act & Assert
            sut.Invoking(x => x.WithType(type: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("type");
        }

        [Fact]
        public void Adds_Correct_Information()
        {
            // Arrange
            var sut = new ClassMethodBuilder();

            // Act
            var result = sut.WithType(typeof(int));

            // Assert
            result.TypeName.Should().Be("System.Int32");
        }
    }
}
