namespace ClassFramework.Pipelines.Tests;

public class BuilderPipelineBuilderTests
{
    [Fact]
    public void Can_Build_Pipeline()
    {
        // Arrange
        var builder = new BuilderPipelineBuilder();

        // Act
        var result = builder.Build();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Can_Alter_Pipeline_Using_Builder()
    {
        // Arrange
        var sourcePipeline = new BuilderPipelineBuilder().Build();

        // Act
        var pipeline = new BuilderPipelineBuilder(sourcePipeline)
            .With(x => x.Features.Clear())
            .Build();

        // Assert
        pipeline.Features.Should().BeEmpty();
    }

    [Fact]
    public void Process_Makes_All_Properties_Writable()
    {
        // Arrange
        var model = new ClassBuilder()
            .AddProperties(
                new ClassPropertyBuilder().WithName("Property1").WithHasSetter(false),
                new ClassPropertyBuilder().WithName("Property2").WithHasSetter(true)
            );
        var context = new BuilderPipelineBuilderSettings();
        var pipeline = new BuilderPipelineBuilder().Build();

        // Act
        pipeline.Process(model, context);

        // Assert
        model.Properties.Select(x => x.HasSetter).Should().AllBeEquivalentTo(true);
    }
}
