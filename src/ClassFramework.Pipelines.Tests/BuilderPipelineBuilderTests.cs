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
        private BuilderPipelineBuilderSettings Context { get; } = new BuilderPipelineBuilderSettings(new BuilderPipelineBuilderNameSettings(builderNamespaceFormatString: "{Namespace}.Builders"));
        private Pipeline<ClassBuilder, BuilderPipelineBuilderSettings> Pipeline { get; } = new BuilderPipelineBuilder().Build();
        private ClassBuilder model { get; } = CreateModel();

        [Fact]
        public void Makes_All_Properties_Writable()
        {
            // Act
            Pipeline.Process(model, Context);

            // Assert
            model.Properties.Select(x => x.HasSetter).Should().AllBeEquivalentTo(true);
        }

        [Fact]
        public void Changes_Namespace_And_Name_According_To_Settings()
        {
            // Act
            Pipeline.Process(model, Context);

            // Assert
            model.Name.Should().Be("MyClassBuilder");
            model.Namespace.Should().Be("MyNamespace.Builders");
        }

        private static ClassBuilder CreateModel()
            => new ClassBuilder()
                .WithName("MyClass")
                .WithNamespace("MyNamespace")
                .AddProperties(
                    new ClassPropertyBuilder().WithName("Property1").WithHasSetter(false),
                    new ClassPropertyBuilder().WithName("Property2").WithHasSetter(true)
                );
    }
}
