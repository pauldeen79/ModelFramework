﻿namespace ClassFramework.Pipelines.Tests.Shared.PlaceholderProcessors;

public class ClassPropertyProcessorTests : TestBase<ClassPropertyProcessor>
{
    public class Process : ClassPropertyProcessorTests
    {
        private ClassProperty CreateModel() => new ClassPropertyBuilder().WithName("Delegate").WithType(typeof(List<string>)).Build();

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
        public void Returns_Continue_On_Unknown_Value()
        {
            // Arrange
            var sut = CreateSut();
            var context = new PipelineContext<ClassProperty>(CreateModel());

            // Act
            var result = sut.Process("Placeholder", CultureInfo.InvariantCulture, context, Fixture.Freeze<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory]
        [InlineData("TypeName", "System.Collections.Generic.List<System.String>")]
        [InlineData("TypeName.GenericArguments", "System.String")]
        [InlineData("TypeName.GenericArgumentsWithBrackets", "<System.String>")]
        [InlineData("TypeName.GenericArguments.ClassName", "String")]
        [InlineData("TypeName.ClassName", "List<System.String>")]
        [InlineData("TypeName.Namespace", "System.Collections.Generic")]
        [InlineData("TypeName.NoGenerics", "System.Collections.Generic.List")]
        [InlineData("Name", "Delegate")]
        [InlineData("NameLower", "delegate")]
        [InlineData("NameUpper", "DELEGATE")]
        [InlineData("NamePascal", "delegate")]
        [InlineData("NamePascalCsharpFriendlyName", "@delegate")]
        [InlineData("DefaultValue", "default(System.Collections.Generic.List<System.String>)")]
        public void Returns_Ok_With_Correct_Value_On_Known_Value(string value, string expectedValue)
        {
            // Arrange
            var formattableStringParser = InitializeParser();
            var sut = CreateSut();
            var settingsMock = Fixture.Freeze<IPropertyGenerationSettings>();
            var model = CreateModel();
            var context = new ClassPropertyContext(model, settingsMock, CultureInfo.InvariantCulture, model.TypeName);

            // Act
            var result = sut.Process(value, CultureInfo.InvariantCulture, context, formattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(expectedValue);
        }
    }
}
