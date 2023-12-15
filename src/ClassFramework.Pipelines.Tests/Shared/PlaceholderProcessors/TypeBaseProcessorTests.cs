﻿namespace ClassFramework.Pipelines.Tests.Shared.PlaceholderProcessors;

public class TypeBaseProcessorTests : TestBase<TypeBaseProcessor>
{
    public class Process : TypeBaseProcessorTests
    {
        private ClassBuilder Model { get; } = new();

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
            var context = new PipelineContext<ClassBuilder, BuilderContext>(Model, new BuilderContext(CreateModel(), new Pipelines.Builder.PipelineBuilderSettings(), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory]
        [InlineData("Name", "SomeClass")]
        [InlineData("NameLower", "someclass")]
        [InlineData("NameUpper", "SOMECLASS")]
        [InlineData("NamePascal", "someClass")]
        [InlineData("Namespace", "SomeNamespace")]
        [InlineData("FullName", "SomeNamespace.SomeClass")]
        [InlineData("Class.Name", "SomeClass")]
        [InlineData("Class.NameLower", "someclass")]
        [InlineData("Class.NameUpper", "SOMECLASS")]
        [InlineData("Class.NamePascal", "someClass")]
        [InlineData("Class.Namespace", "SomeNamespace")]
        [InlineData("Class.FullName", "SomeNamespace.SomeClass")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value(string value, string expectedValue)
        {
            // Arrange
            var sut = CreateSut();
            var context = new PipelineContext<IType>(CreateModel());

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }
    }
}
