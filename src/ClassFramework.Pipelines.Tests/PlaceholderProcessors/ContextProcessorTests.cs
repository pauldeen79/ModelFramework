namespace ClassFramework.Pipelines.Tests.PlaceholderProcessors;

public class ContextProcessorTests : TestBase<ContextProcessor>
{
    public class Process : ContextProcessorTests
    {
        private ClassBuilder Model { get; } = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace");

        [Fact]
        public void Returns_Continue_When_Context_Is_Not_PipelineContext()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, null, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Continue_On_Unknown_Value()
        {
            // Arrange
            var sut = CreateSut();
            var context = new PipelineContext<TypeBuilder, BuilderPipelineBuilderSettings>(Model, new BuilderPipelineBuilderSettings());

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory]
        [InlineData("Name", "MyClass")]
        [InlineData("NameLower", "myclass")]
        [InlineData("NameUpper", "MYCLASS")]
        [InlineData("NamePascal", "myClass")]
        [InlineData("Namespace", "MyNamespace")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value(string value, string expectedValue)
        {
            // Arrange
            var sut = CreateSut();
            var context = new PipelineContext<TypeBuilder, BuilderPipelineBuilderSettings>(Model, new BuilderPipelineBuilderSettings());

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }
    }
}
