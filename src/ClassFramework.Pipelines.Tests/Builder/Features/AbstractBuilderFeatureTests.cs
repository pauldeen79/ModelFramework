namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AbstractBuilderFeatureTests : TestBase<AbstractBuilderFeature>
{
    public class Process : AbstractBuilderFeatureTests
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
        public void Adds_AddGenericTypeArguments_When_IsBuilderForAbstractEntity_Is_True()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").Build();
            var parser = Fixture.Freeze<IFormattableStringParser>();
            parser.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                  .Returns(x => Result<string>.Success(x.ArgAt<string>(0).Replace("{Name}", sourceModel.Name, StringComparison.Ordinal)));
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(inheritanceSettings: new PipelineBuilderInheritanceSettings(enableEntityInheritance: true));
            var context = new PipelineContext<ClassBuilder, PipelineBuilderContext>(model, new PipelineBuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Interfaces.Should().BeEmpty();
            model.GenericTypeArguments.Should().BeEquivalentTo("TBuilder", "TEntity");
            model.GenericTypeArgumentConstraints.Should().BeEquivalentTo("where TEntity : SomeNamespace.SomeClass", "where TBuilder : SomeClassBuilder<TBuilder, TEntity>");
            model.Abstract.Should().BeTrue();
        }

        [Fact]
        public void Adds_IValidatableObject_Interface_When_IsBuilderForAbstractEntity_Is_False_And_Validation_Is_Shared_Between_Builder_And_Entity()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(classSettings: new ImmutableClassPipelineBuilderSettings(constructorSettings: new ImmutableClassPipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.Shared)));
            var context = new PipelineContext<ClassBuilder, PipelineBuilderContext>(model, new PipelineBuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Interfaces.Should().BeEquivalentTo("System.ComponentModel.DataAnnotations.IValidatableObject");
            model.GenericTypeArguments.Should().BeEmpty();
            model.GenericTypeArgumentConstraints.Should().BeEmpty();
            model.Abstract.Should().BeFalse();
        }

        [Fact]
        public void Does_Not_Add_IValidatableObject_Interface_When_IsBuilderForAbstractEntity_Is_False_And_Validation_Is_Not_Shared_Between_Builder_And_Entity()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings();
            var context = new PipelineContext<ClassBuilder, PipelineBuilderContext>(model, new PipelineBuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Interfaces.Should().BeEmpty();
            model.GenericTypeArguments.Should().BeEmpty();
            model.GenericTypeArgumentConstraints.Should().BeEmpty();
            model.Abstract.Should().BeFalse();
        }
    }
}
