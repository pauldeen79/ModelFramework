namespace ClassFramework.Domain.Builders.Tests.Types;

public class ClassBuilderTests
{
    public class WithBaseClass
    {
        [Fact]
        public void WithBaseClass_Throws_On_Null_BaseClassType()
        {
            // Arrange
            var sut = new ClassBuilder();

            // Act & Assert
            sut.Invoking(x => x.WithBaseClass(baseClassType: default!))
               .Should().Throw<ArgumentNullException>().WithParameterName("baseClassType");
        }

        [Fact]
        public void WithBaseClass_Returns_Correct_Result()
        {
            // Arrange
            var sut = new ClassBuilder();

            // Act
            var result = sut.WithBaseClass(typeof(string));

            // Assert
            result.BaseClass.Should().Be(typeof(string).FullName);
        }
    }
}
