namespace ClassFramework.Pipelines.Tests.Entity.Features;

public class AbstractEntityFeatureTests : TestBase<Pipelines.Entity.Features.AbstractEntityFeature>
{
    public class Process : AbstractEntityFeatureTests
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
        public void Updates_IsAbstract_To_True_When_SourceModel_Is_Abstract()
        {
            // Arrange
            var sourceModel = CreateModel(baseClass: string.Empty);
            var sut = CreateSut();
            var model = new ClassBuilder().WithAbstract(false); // we want to make sure that the component updates the property
            var settings = CreateEntitySettings(
                enableEntityInheritance: true,
                isAbstract: true);
            var context = new PipelineContext<TypeBaseBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Abstract.Should().BeTrue();
        }

        [Fact]
        public void Updates_IsAbstract_To_False_When_SourceModel_Is_Not_Abstract()
        {
            // Arrange
            var sourceModel = CreateModel(baseClass: string.Empty);
            var sut = CreateSut();
            var model = new ClassBuilder().WithAbstract(true); // we want to make sure that the component updates the property
            var settings = CreateEntitySettings(
                enableEntityInheritance: true,
                isAbstract: false);
            var context = new PipelineContext<TypeBaseBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Abstract.Should().BeFalse();
        }
    }
}
