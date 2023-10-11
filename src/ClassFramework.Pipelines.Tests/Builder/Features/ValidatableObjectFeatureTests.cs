namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class ValidatableObjectFeatureTests : TestBase<ValidatableObjectFeature>
{
    public class Process : ValidatableObjectFeatureTests
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
        public void Adds_IValidatableObject_Interface_When_IsBuilderForAbstractEntity_Is_False_And_Validation_Is_Shared_Between_Builder_And_Entity()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(classSettings: new ImmutableClassPipelineBuilderSettings(constructorSettings: new ImmutableClassPipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.Shared)));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Interfaces.Should().BeEquivalentTo("System.ComponentModel.DataAnnotations.IValidatableObject");
        }

        [Fact]
        public void Does_Not_Add_IValidatableObject_Interface_When_IsBuilderForAbstractEntity_Is_False_And_Validation_Is_Not_Shared_Between_Builder_And_Entity()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Interfaces.Should().BeEmpty();
        }
    }
}
