namespace ClassFramework.Domain.Tests.Builders.Extensions;

public class BaseClassContainerBuilderExtensionsTests
{
    public class WithBaseClass
    {
        [Fact]
        public void Throws_On_Null_BaseClassType()
        {
            // Arrange
            var sut = new ClassBuilder();

            // Act & Assert
            sut.Invoking(x => _ = sut.WithBaseClass(baseClassType: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("baseClassType");
        }
    }
}
