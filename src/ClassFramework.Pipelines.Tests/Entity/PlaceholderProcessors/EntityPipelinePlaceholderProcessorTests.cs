namespace ClassFramework.Pipelines.Tests.Entity.PlaceholderProcessors;

public class EntityPipelinePlaceholderProcessorTests : TestBase<EntityPipelinePlaceholderProcessor>
{
    public class Process : EntityPipelinePlaceholderProcessorTests
    {
        private ClassBuilder CreateModel() => new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace");

        [Fact]
        public void Throws_On_Null_FormattableStringParser()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Process("Placeholder", CultureInfo.InvariantCulture, null, formattableStringParser: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }

        [Fact]
        public void PipelineContext()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, null, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Result_From_PropertyPlaceholderProcessor_On_Unknown_Value()
        {
            // Arrange
            var propertyPlaceholderProcessor = Fixture.Freeze<IPipelinePlaceholderProcessor>();
            var externalResult = Result.NoContent<string>();
            propertyPlaceholderProcessor.Process(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>(), Arg.Any<IFormattableStringParser>()).Returns(externalResult);
            var sut = CreateSut();
            var context = new PipelineContext<TypeBaseBuilder, EntityContext>(CreateModel(), new EntityContext(CreateModel().Build(), new Pipelines.Entity.PipelineBuilderSettings(), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Should().BeSameAs(externalResult);
        }

        [Theory]
        [InlineData("EntityNamespace", "MyNamespace")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value_With_PipelineContext(string value, string expectedValue)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("MyEntityNamespaceFormatString", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result.Success("MyNamespace"));
            var sut = CreateSut();
            var context = new PipelineContext<TypeBaseBuilder, EntityContext>(CreateModel(), new EntityContext(CreateModel().Build(), new Pipelines.Entity.PipelineBuilderSettings(nameSettings: new Pipelines.Entity.PipelineBuilderNameSettings(entityNamespaceFormatString: "MyEntityNamespaceFormatString")), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, formattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(ArgumentValidationType.Shared, "Base")]
        [InlineData(ArgumentValidationType.DomainOnly, "")]
        [InlineData(ArgumentValidationType.None, "")]
        public void Returns_Ok_With_Correct_Value_On_EntityNameSuffix_Based_On_ValidateArguments(ArgumentValidationType validateArguments, string expectedValue)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            var sut = CreateSut();
            var context = new PipelineContext<TypeBaseBuilder, EntityContext>(CreateModel(), new EntityContext(CreateModel().Build(), new Pipelines.Entity.PipelineBuilderSettings(constructorSettings: new Pipelines.Entity.PipelineBuilderConstructorSettings(validateArguments: validateArguments)), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process("EntityNameSuffix", CultureInfo.InvariantCulture, context, formattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }
    }
}
