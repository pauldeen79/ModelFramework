﻿namespace ClassFramework.Pipelines.Tests.Builder.PlaceholderProcessors;

public class ParentClassPropertyChildContextProcessorTests : TestBase<ParentClassPropertyChildContextProcessor>
{
    public class Process : ParentClassPropertyChildContextProcessorTests
    {
        private ClassProperty PropertyModel { get; } = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(List<string>)).Build();
        private ClassBuilder CreateModel() => new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace");

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
        public void Returns_Continue_On_Unknown_Value()
        {
            // Arrange
            var sut = CreateSut();
            var context = new ParentChildContext<ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new PipelineBuilderSettings(), CultureInfo.InvariantCulture)), PropertyModel);

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory]
        [InlineData("NullCheck.Source", "if (source.MyProperty is not null) ")] // null checks are enabled in this unit test
        [InlineData("BuildersNamespace", "MyNamespace.Builders")]
        [InlineData("TypeName", "System.Collections.Generic.List<System.String>")]
        [InlineData("TypeName.GenericArguments", "System.String")]
        [InlineData("TypeName.GenericArgumentsWithBrackets", "<System.String>")]
        [InlineData("TypeName.GenericArguments.ClassName", "String")]
        [InlineData("TypeName.ClassName", "List<System.String>")]
        [InlineData("TypeName.Namespace", "System.Collections.Generic")]
        [InlineData("TypeName.NoGenerics", "System.Collections.Generic.List")]
        [InlineData("Name", "MyProperty")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value_Without_FormatInstanceTypeNameDelegate(string value, string expectedValue)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("{Namespace}.Builders", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result<string>.Success("MyNamespace.Builders"));
            var sut = CreateSut();
            var context = new ParentChildContext<ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true)), CultureInfo.InvariantCulture)), PropertyModel);

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("NullCheck.Source", "")] // null checks are disabled in this unit test
        [InlineData("BuildersNamespace", "MyNamespace.Builders")]
        [InlineData("TypeName", "delegate(MyClass,True)")]
        [InlineData("TypeName.GenericArguments", "")]
        [InlineData("TypeName.GenericArgumentsWithBrackets", "")]
        [InlineData("TypeName.GenericArguments.ClassName", "")]
        [InlineData("TypeName.ClassName", "delegate(MyClass,True)")]
        [InlineData("TypeName.Namespace", "")]
        [InlineData("TypeName.NoGenerics", "delegate(MyClass,True)")]
        [InlineData("Name", "MyProperty")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value_With_FormatInstanceTypeNameDelegate(string value, string expectedValue)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("{Namespace}.Builders", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result<string>.Success("MyNamespace.Builders"));
            var sut = CreateSut();
            var context = new ParentChildContext<ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new PipelineBuilderSettings(typeSettings: new PipelineBuilderTypeSettings(formatInstanceTypeNameDelegate: (type, forCreate) => $"delegate({type.Name},{forCreate})")), CultureInfo.InvariantCulture)), PropertyModel);

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }
    }
}