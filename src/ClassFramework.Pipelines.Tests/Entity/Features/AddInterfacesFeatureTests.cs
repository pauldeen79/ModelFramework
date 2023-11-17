namespace ClassFramework.Pipelines.Tests.Entity.Features;

public class AddInterfacesFeatureTests : TestBase<Pipelines.Entity.Features.AddInterfacesFeature>
{
    public class Process : AddInterfacesFeatureTests
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
        public void Adds_Interfaces_When_CopyInterfacePredicate_Setting_Is_Not_Null()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddInterfaces("IMyInterface").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(copyInterfacePredicate: _ => true);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().BeEquivalentTo("IMyInterface");
        }

        [Fact]
        public void Does_Not_Interfaces_When_CopyInterfacePredicate_Setting_Is_Null()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddInterfaces("IMyInterface").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(copyInterfacePredicate: null);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().BeEmpty();
        }
    }
}
