namespace ClassFramework.Pipelines.Tests.Reflection.Features;

public class AddConstructorsFeatureTests : TestBase<Pipelines.Reflection.Features.AddConstructorsFeature>
{
    public class Process : AddConstructorsFeatureTests
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
        public void Does_Not_Add_Constructors_When_CreateConstructors_Is_Set_To_False()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyConstructorTestClass);
            var model = new ClassBuilder();
            var settings = CreateReflectionSettings(createConstructors: false);
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Constructors.Should().BeEmpty();
        }

        [Fact]
        public void Does_Not_Add_Constructors_When_SourceModel_Is_Of_Type_Interface()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyConstructorTestClass);
            var model = new InterfaceBuilder();
            var settings = CreateReflectionSettings(createConstructors: false);
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            // can't even check constructors on model, because an interface does not have constructors
        }
    }
}

internal sealed class MyConstructorTestClass
{
    public MyConstructorTestClass()
    {
    }

    public MyConstructorTestClass(int value)
    {
    }
}

internal interface IMyConstructorTestInterface
{
}
