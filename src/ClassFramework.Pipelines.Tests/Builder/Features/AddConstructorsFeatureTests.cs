namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddConstructorsFeatureTests : TestBase<AddConstructorsFeature>
{
    public class Process : AddConstructorsFeatureTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Process(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }
    }
}
