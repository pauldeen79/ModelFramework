namespace ClassFramework.Pipelines.Tests;

public class BuilderPipelineBuilderTests
{
    public class Build
    {
        [Fact]
        public void Builds_Pipeline()
        {
            // Arrange
            var builder = new BuilderPipelineBuilder();

            // Act
            var result = builder.Build();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Builds_Altered_Pipeline_Using_Existing_Pipeline()
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
    }

    public class Process
    {
        [Fact]
        public void Makes_All_Properties_Writable()
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
}
