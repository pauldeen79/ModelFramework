namespace ClassFramework.Pipelines.Tests.Builder.PlaceholderProcessors;

public class BuilderPipelinePlaceholderProcessorTests : TestBase<BuilderPipelinePlaceholderProcessor>
{
    public class Process : BuilderPipelinePlaceholderProcessorTests
    {
        private ClassProperty CreatePropertyModel(bool isNullable = false) => new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(List<string>)).WithIsNullable(isNullable).Build();
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
        public void Returns_Continue_When_Context_Is_Not_ParentChildContext()
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
            var context = new ParentChildContext<BuilderContext, ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new Pipelines.Builder.PipelineBuilderSettings(), CultureInfo.InvariantCulture)), CreatePropertyModel());

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Should().BeSameAs(externalResult);
        }

        [Theory]
        [InlineData("NullCheck.Source", "if (source.MyProperty is not null) ")] // null checks are enabled in this unit test
        [InlineData("NullCheck.Argument", "if (myProperty is null) throw new System.ArgumentNullException(nameof(myProperty));")] // null checks are enabled in this unit test
        [InlineData("BuildersNamespace", "MyNamespace.Builders")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value_With_ParentChildContext_With_NullChecks_Enabled(string value, string expectedValue)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("{Namespace}.Builders", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result.Success("MyNamespace.Builders"));
            var sut = CreateSut();
            var context = new ParentChildContext<BuilderContext, ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new Pipelines.Builder.PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true)), CultureInfo.InvariantCulture)), CreatePropertyModel());

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("NullCheck.Source", "")]
        [InlineData("NullCheck.Argument", "")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value_With_ParentChildContext_Without_NullChecks(string value, string expectedValue)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("{Namespace}.Builders", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result.Success("MyNamespace.Builders"));
            var sut = CreateSut();
            var context = new ParentChildContext<BuilderContext, ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new Pipelines.Builder.PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: false)), CultureInfo.InvariantCulture)), CreatePropertyModel());

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("BuildersNamespace", "MyNamespace.Builders")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value_With_PipelineContext(string value, string expectedValue)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("{Namespace}.Builders", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result.Success("MyNamespace.Builders"));
            var sut = CreateSut();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new Pipelines.Builder.PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true)), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("NullableRequiredSuffix", true, "")]
        [InlineData("NullableRequiredSuffix", false, "!")]
        [InlineData("NullableSuffix", true, "?")]
        [InlineData("NullableSuffix", false, "")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value_Depending_On_IsNullable(string value, bool isNullable, string expectedResult)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("{Namespace}.Builders", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result.Success("MyNamespace.Builders"));
            var sut = CreateSut();
            var context = new ParentChildContext<BuilderContext, ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new Pipelines.Builder.PipelineBuilderSettings(
                generationSettings: new PipelineBuilderGenerationSettings(enableNullableReferenceTypes: true)
                ) , CultureInfo.InvariantCulture)), CreatePropertyModel(isNullable));

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedResult);
        }

        [Fact]
        public void Returns_Result_When_PipelinePlaceholderProcessor_Supports_The_Value_With_ParentChildContext()
        {
            // Arrange
            var pipelinePlaceholderProcessor = Fixture.Freeze<IPipelinePlaceholderProcessor>();
            pipelinePlaceholderProcessor.Process("Value", Arg.Any<IFormatProvider>(), Arg.Any<object?>(), Arg.Any<IFormattableStringParser>()).Returns(Result.Success("MyResult"));
            var sut = CreateSut();
            var context = new ParentChildContext<BuilderContext, ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new Pipelines.Builder.PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true)), CultureInfo.InvariantCulture)), CreatePropertyModel());

            // Act
            var result = sut.Process("Value", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("MyResult");
        }

        [Fact]
        public void Returns_Continue_When_PipelinePlaceholderProcessor_Does_Not_Support_The_Value_With_ParentChildContext()
        {
            // Arrange
            var pipelinePlaceholderProcessor = Fixture.Freeze<IPipelinePlaceholderProcessor>();
            pipelinePlaceholderProcessor.Process("Value", Arg.Any<IFormatProvider>(), Arg.Any<object?>(), Arg.Any<IFormattableStringParser>()).Returns(Result.Continue<string>());
            var sut = CreateSut();
            var context = new ParentChildContext<BuilderContext, ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new Pipelines.Builder.PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true)), CultureInfo.InvariantCulture)), CreatePropertyModel());

            // Act
            var result = sut.Process("Value", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Result_When_PipelinePlaceholderProcessor_Supports_The_Value_With_PipelineChildContext()
        {
            // Arrange
            var pipelinePlaceholderProcessor = Fixture.Freeze<IPipelinePlaceholderProcessor>();
            pipelinePlaceholderProcessor.Process("Value", Arg.Any<IFormatProvider>(), Arg.Any<object?>(), Arg.Any<IFormattableStringParser>()).Returns(Result.Success("MyResult"));
            var sut = CreateSut();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new Pipelines.Builder.PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true)), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process("Value", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("MyResult");
        }

        [Fact]
        public void Returns_Continue_When_PipelinePlaceholderProcessor_Does_Not_Support_The_Value_With_PipelineChildContext()
        {
            // Arrange
            var pipelinePlaceholderProcessor = Fixture.Freeze<IPipelinePlaceholderProcessor>();
            pipelinePlaceholderProcessor.Process("Value", Arg.Any<IFormatProvider>(), Arg.Any<object?>(), Arg.Any<IFormattableStringParser>()).Returns(Result.Continue<string>());
            var sut = CreateSut();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new Pipelines.Builder.PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true)), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process("Value", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }
    }
}
