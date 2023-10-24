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
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true)));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.GenericTypeArguments.Should().BeEquivalentTo("TBuilder", "TEntity");
            model.GenericTypeArgumentConstraints.Should().BeEquivalentTo("where TEntity : SomeNamespace.SomeClass", "where TBuilder : SomeClassBuilder<TBuilder, TEntity>");
            model.Abstract.Should().BeTrue();
        }

        [Fact]
        public void Does_Not_Add_AddGenericTypeArguments_When_IsBuilderForAbstractEntity_Is_False_And_Validation_Is_Not_Shared_Between_Builder_And_Entity()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.GenericTypeArguments.Should().BeEmpty();
            model.GenericTypeArgumentConstraints.Should().BeEmpty();
            model.Abstract.Should().BeFalse();
        }
    }
}
