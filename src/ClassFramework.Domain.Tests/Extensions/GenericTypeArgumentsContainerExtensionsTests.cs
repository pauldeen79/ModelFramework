namespace ClassFramework.Domain.Tests.Extensions;

public class GenericTypeArgumentsContainerExtensionsTests : TestBase
{
    public class GetGenericTypeArgumentsString : GenericTypeArgumentsContainerExtensionsTests
    {
        [Fact]
        public void Returns_Empty_String_When_No_GenericArguments_Are_Present()
        {
            // Arrange
            var sut = Fixture.Freeze<IGenericTypeArgumentsContainer>();
            sut.GenericTypeArguments.Returns(Array.Empty<string>());

            // Act
            var result = sut.GetGenericTypeArgumentsString();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_When_GenericArguments_Are_Present()
        {
            // Arrange
            var sut = Fixture.Freeze<IGenericTypeArgumentsContainer>();
            sut.GenericTypeArguments.Returns(["T"]);


            // Act
            var result = sut.GetGenericTypeArgumentsString();

            // Assert
            result.Should().Be("<T>");
        }
    }
}
