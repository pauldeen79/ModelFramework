namespace ClassFramework.Pipelines.Tests.Shared.PlaceholderProcessors;

public class ClassPropertyProcessorTests : TestBase<ClassPropertyProcessor>
{
    public class Process : ClassPropertyProcessorTests
    {
        private ClassProperty CreateModel() => new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(List<string>)).Build();
        
        [Fact]
        public void Returns_Continue_When_Context_Is_Not_ParentChildContext()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, null, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.IsSuccessful().Should().BeTrue();
        }

        [Theory]
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
        public void Returns_Ok_With_Correct_Value_On_Known_Value(string value, string expectedValue)
        {
            // Arrange
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();
            formattableStringParser.Parse("{Namespace}.Builders", Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(Result.Success("MyNamespace.Builders"));
            var sut = CreateSut();
            var context = new PipelineContext<ClassProperty>(CreateModel());

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }
    }
}
