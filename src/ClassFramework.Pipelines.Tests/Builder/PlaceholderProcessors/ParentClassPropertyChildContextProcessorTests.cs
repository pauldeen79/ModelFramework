namespace ClassFramework.Pipelines.Tests.Builder.PlaceholderProcessors;

public class ParentClassPropertyChildContextProcessorTests : TestBase<ParentClassPropertyChildContextProcessor>
{
    public class Process : ParentClassPropertyChildContextProcessorTests
    {
        private ClassProperty CreatePropertyModel(bool isNullable = false) => new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(List<string>)).WithIsNullable(isNullable).Build();
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
            var context = new ParentChildContext<ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new PipelineBuilderSettings(), CultureInfo.InvariantCulture)), CreatePropertyModel());

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
        [InlineData("NameLower", "myproperty")]
        [InlineData("NameUpper", "MYPROPERTY")]
        [InlineData("NamePascal", "myProperty")]
        [InlineData("Class.Name", "MyClass")]
        [InlineData("Class.NameLower", "myclass")]
        [InlineData("Class.NameUpper", "MYCLASS")]
        [InlineData("Class.NamePascal", "myClass")]
        [InlineData("Class.Namespace", "MyNamespace")]
        [InlineData("Class.FullName", "MyNamespace.MyClass")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value(string value, string expectedValue)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("{Namespace}.Builders", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result.Success("MyNamespace.Builders"));
            var sut = CreateSut();
            var context = new ParentChildContext<ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true)), CultureInfo.InvariantCulture)), CreatePropertyModel());

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("NullableRequiredSuffix", true, "")]
        [InlineData("NullableRequiredSuffix", false, "!")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value_Depending_On_IsNullable(string value, bool isNullable, string expectedResult)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("{Namespace}.Builders", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result.Success("MyNamespace.Builders"));
            var sut = CreateSut();
            var context = new ParentChildContext<ClassProperty>(new PipelineContext<ClassBuilder, BuilderContext>(CreateModel(), new BuilderContext(CreateModel().Build(), new PipelineBuilderSettings(
                generationSettings: new PipelineBuilderGenerationSettings(enableNullableReferenceTypes: true)
                ) , CultureInfo.InvariantCulture)), CreatePropertyModel(isNullable));

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedResult);
        }
    }
}
