namespace ClassFramework.Pipelines.Tests.Entity.Features;

public class SetNameFeatureTests : TestBase<Pipelines.Entity.Features.SetNameFeature>
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
        public void Sets_Name_Property_With_Default_FormatString()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings();
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Name.Should().Be("SomeClass/* suffix goes here*/");
        }

        [Fact]
        public void Sets_Name_Property_With_Custom_FormatString()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(entityNameFormatString: "CustomClassName{EntityNameSuffix}");
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Name.Should().Be("CustomClassName/* suffix goes here*/");
        }

        [Fact]
        public void Sets_Namespace_Property_With_Default_FormatString()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings();
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Namespace.Should().Be("SomeNamespace");
        }

        [Fact]
        public void Sets_Namespace_Property_With_Custom_FormatString()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(entityNamespaceFormatString: "CustomNamespace");
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Namespace.Should().Be("CustomNamespace");
        }

        [Fact]
        public void Returns_Error_When_Parsing_NameFormatString_Is_Not_Successful()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(entityNameFormatString: "{Error}");
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }
    }
}
