namespace ClassFramework.Pipelines.Tests;

public class BuilderPipelineBuilderTests
{
    [Fact]
    public void Can_Build_Pipeline()
    {
        // Arrange
        var sut = new BuilderPipelineBuilder();

        // Act
        var result = sut.Build();

        // Assert
        result.Should().NotBeNull();
    }
}
