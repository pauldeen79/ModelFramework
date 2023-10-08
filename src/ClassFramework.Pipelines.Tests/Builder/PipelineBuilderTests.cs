namespace ClassFramework.Pipelines.Tests.Builder;

public class PipelineBuilderTests : IDisposable
{
    private bool disposedValue;
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    public PipelineBuilderTests()
    {
        _provider = new ServiceCollection()
            .AddParsers()
            .AddPipelines()
            .BuildServiceProvider();
        _scope = _provider.CreateScope();
    }

    protected IPipelineBuilder<ClassBuilder, BuilderContext> CreateSut() => _scope.ServiceProvider.GetRequiredService<IPipelineBuilder<ClassBuilder, BuilderContext>>();

    public class Constructor : PipelineBuilderTests
    {
        [Fact]
        public void Allows_Altering_Existing_Pipeline()
        {
            // Arrange
            var sourcePipeline = CreateSut().Build();

            // Act
            var pipeline = new PipelineBuilder(sourcePipeline)
                .With(x => x.Features.Clear())
                .Build();

            // Assert
            pipeline.Features.Should().BeEmpty();
        }
    }

    public class Process : PipelineBuilderTests
    {
        private BuilderContext Context { get; } = new BuilderContext(CreateModel(), new PipelineBuilderSettings(new PipelineBuilderNameSettings(builderNamespaceFormatString: "{Namespace}.Builders")), CultureInfo.InvariantCulture);
        private ClassBuilder Model { get; } = new();

        [Fact]
        public void Sets_Partial()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            sut.Process(Model, Context);

            // Assert
            Model.Partial.Should().BeTrue();
        }

        [Fact]
        public void Sets_Namespace_And_Name_According_To_Settings()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            sut.Process(Model, Context);

            // Assert
            Model.Name.Should().Be("MyClassBuilder");
            Model.Namespace.Should().Be("MyNamespace.Builders");
        }

        [Fact]
        public void Adds_Properties()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            sut.Process(Model, Context);

            // Assert
            Model.Properties.Select(x => x.HasSetter).Should().AllBeEquivalentTo(true);
            Model.Properties.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
            Model.Properties.Select(x => x.TypeName).Should().BeEquivalentTo("System.String", "System.String");
        }

        private static TypeBase CreateModel()
            => new ClassBuilder()
                .WithName("MyClass")
                .WithNamespace("MyNamespace")
                .AddProperties(
                    new ClassPropertyBuilder().WithName("Property1").WithTypeName("System.String").WithHasSetter(false),
                    new ClassPropertyBuilder().WithName("Property2").WithTypeName("System.String").WithHasSetter(true))
                .Build();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _scope.Dispose();
                _provider.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
