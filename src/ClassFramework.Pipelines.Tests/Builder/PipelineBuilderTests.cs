namespace ClassFramework.Pipelines.Tests.Builder;

public class PipelineBuilderTests
{
    public class Build
    {
        [Fact]
        public void Builds_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder();

            // Act
            var result = builder.Build();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Builds_Altered_Pipeline_Using_Existing_Pipeline()
        {
            // Arrange
            var sourcePipeline = new PipelineBuilder().Build();

            // Act
            var pipeline = new PipelineBuilder(sourcePipeline)
                .With(x => x.Features.Clear())
                .Build();

            // Assert
            pipeline.Features.Should().BeEmpty();
        }
    }

    public class Process
    {
        private PipelineBuilderContext Context { get; } = new PipelineBuilderContext(CreateModel().Build(), new PipelineBuilderSettings(new PipelineBuilderNameSettings(builderNamespaceFormatString: "{Namespace}.Builders")), CultureInfo.InvariantCulture);
        private Pipeline<ClassBuilder, PipelineBuilderContext> Pipeline { get; } = new PipelineBuilder().Build();
        private ClassBuilder Model { get; } = new();

        [Fact]
        public void Sets_Partial()
        {
            // Act
            Pipeline.Process(Model, Context);

            // Assert
            Model.Partial.Should().BeTrue();
        }

        [Fact]
        public void Sets_Namespace_And_Name_According_To_Settings()
        {
            // Act
            Pipeline.Process(Model, Context);

            // Assert
            Model.Name.Should().Be("MyClassBuilder");
            Model.Namespace.Should().Be("MyNamespace.Builders");
        }

        [Fact]
        public void Adds_Properties()
        {
            // Act
            Pipeline.Process(Model, Context);

            // Assert
            Model.Properties.Select(x => x.HasSetter).Should().AllBeEquivalentTo(true);
            Model.Properties.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
            Model.Properties.Select(x => x.TypeName).Should().BeEquivalentTo("System.String", "System.String");
        }

        private static ClassBuilder CreateModel()
            => new ClassBuilder()
                .WithName("MyClass")
                .WithNamespace("MyNamespace")
                .AddProperties(
                    new ClassPropertyBuilder().WithName("Property1").WithTypeName("System.String").WithHasSetter(false),
                    new ClassPropertyBuilder().WithName("Property2").WithTypeName("System.String").WithHasSetter(true)
                );
    }
}
