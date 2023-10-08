namespace ClassFramework.Pipelines.Tests.Builder.PlaceholderProcessors;

public class ClassPropertyProcessorTests : TestBase<ClassPropertyProcessor>
{
    public class Process : ClassPropertyProcessorTests
    {
        private ClassProperty Model { get; } = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(List<string>)).Build();
        private TypeBase CreateModel() => new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

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
            var context = new PipelineContext<ClassProperty, BuilderContext>(Model, new BuilderContext(CreateModel(), new PipelineBuilderSettings(), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory]
        [InlineData("TypeName", "System.Collections.Generic.List<System.String>")]
        [InlineData("TypeName.GenericArguments", "System.String")]
        [InlineData("TypeName.ClassName", "List<System.String>")]
        [InlineData("TypeName.GenericArguments.ClassName", "String")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value(string value, string expectedValue)
        {
            // Arrange
            var sut = CreateSut();
            var context = new PipelineContext<ClassProperty, BuilderContext>(Model, new BuilderContext(CreateModel(), new PipelineBuilderSettings(), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }
    }
}
