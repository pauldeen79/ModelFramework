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

    protected IPipelineBuilder<ClassBuilder, BuilderContext> CreateSut()
        => _scope.ServiceProvider.GetRequiredService<IPipelineBuilder<ClassBuilder, BuilderContext>>();

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
        private BuilderContext CreateContext(bool addProperties = true) => new BuilderContext
            (
                CreateModel(addProperties),
                new PipelineBuilderSettings
                (
                    nameSettings: new PipelineBuilderNameSettings(builderNamespaceFormatString: "{Namespace}.Builders"),
                    classSettings: new ImmutableClassPipelineBuilderSettings(allowGenerationWithoutProperties: false),
                    generationSettings: new PipelineBuilderGenerationSettings(copyAttributes: true)
                ),
                CultureInfo.InvariantCulture
            );

        private ClassBuilder Model { get; } = new();

        [Fact]
        public void Sets_Partial()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            Model.Partial.Should().BeTrue();
        }

        [Fact]
        public void Sets_Namespace_And_Name_According_To_Settings()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            Model.Name.Should().Be("MyClassBuilder");
            Model.Namespace.Should().Be("MyNamespace.Builders");
        }

        [Fact]
        public void Adds_Properties()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            Model.Properties.Select(x => x.HasSetter).Should().AllBeEquivalentTo(true);
            Model.Properties.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
            Model.Properties.Select(x => x.TypeName).Should().BeEquivalentTo("System.String", "System.Collections.Generic.List<System.String>");
        }

        [Fact]
        public void Adds_Constructors()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            Model.Constructors.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Adds_GenericTypeArguments()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            Model.GenericTypeArguments.Should().NotBeEmpty();
        }

        [Fact]
        public void Adds_GenericTypeArgumentConstraints()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            Model.GenericTypeArgumentConstraints.Should().NotBeEmpty();
        }

        [Fact]
        public void Adds_Attributes()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            Model.Attributes.Should().NotBeEmpty();
        }

        [Fact]
        public void Adds_Build_Method()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            Model.Methods.Where(x => x.Name == "Build").Should().ContainSingle();
            var method = Model.Methods.Single(x => x.Name == "Build");
            method.TypeName.Should().Be("MyNamespace.MyClass<T>");
            method.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            method.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo("return new MyNamespace.MyClass<T> { Property2 = Property2 };");
        }

        [Fact]
        public void Adds_Fluent_Method_For_NonCollection_Property()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            Model.Methods.Where(x => x.Name == "WithProperty1").Should().ContainSingle();
            var method = Model.Methods.Single(x => x.Name == "WithProperty1");
            method.TypeName.Should().Be("MyClassBuilder<T>");
            method.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            method.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo("Property1 = property1;", "return this;");

            Model.Methods.Where(x => x.Name == "WithProperty2").Should().BeEmpty(); //only for the non-collection property
        }

        [Fact]
        public void Throws_When_SourceModel_Does_Not_Have_Properties_And_AllowGenerationWithoutProperties_Is_False()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act & Assert
            sut.Invoking(x => x.Process(Model, CreateContext(addProperties: false)))
               .Should().Throw<InvalidOperationException>().WithMessage("To create a builder class, there must be at least one property");
        }

        private static TypeBase CreateModel(bool addProperties)
            => new ClassBuilder()
                .WithName("MyClass")
                .WithNamespace("MyNamespace")
                .AddGenericTypeArguments("T")
                .AddGenericTypeArgumentConstraints("where T : class")
                .AddAttributes(new AttributeBuilder().WithName("MyAttribute"))
                .AddProperties(
                    new[]
                    {
                        new ClassPropertyBuilder().WithName("Property1").WithTypeName("System.String").WithHasSetter(false),
                        new ClassPropertyBuilder().WithName("Property2").WithTypeName(typeof(List<>).WithoutGenerics() + "<System.String>").WithHasSetter(true)
                    }.Where(_ => addProperties)
                )
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
