namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class SetNameFeatureTests : TestBase<Pipelines.Builder.Features.SetNameFeature>
{
    public class Process : SetNameFeatureTests
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

        [Fact]
        public void Sets_Name_Property()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Name.Should().Be("SomeClassBuilder");
        }

        [Fact]
        public void Sets_Namespace_Property()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Namespace.Should().Be("SomeNamespace.Builders");
        }

        [Fact]
        public void Returns_Error_When_Parsing_BuilderNameFormatString_Is_Not_Succesful()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(builderNameFormatString: "{Error}");
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Error_When_Parsing_BuilderNameSpaceFormatString_Is_Not_Succesful()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(builderNamespaceFormatString: "{Error}");
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }
    }
}
