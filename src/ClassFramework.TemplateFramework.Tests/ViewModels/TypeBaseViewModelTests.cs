namespace ClassFramework.TemplateFramework.Tests.ViewModels;

public class TypeBaseViewModelTests : TestBase<TypeBaseViewModel>
{
    public TypeBaseViewModelTests() : base()
    {
        // For some reason, we have to register this class, because else we get the following exception:
        // AutoFixture was unable to create an instance of type AutoFixture.Kernel.SeededRequest because the traversed object graph contains a circular reference
        // I tried a generic fix in TestBase (omitting Model property), but this makes some tests fail and I don't understand why :-(
        Fixture.Register(() => new TypeBaseViewModel(Fixture.Freeze<ICsharpExpressionCreator>()));
    }

    public class ShouldRenderNullablePragmas : TypeBaseViewModelTests
    {
        [Fact]
        public void Throws_When_Settings_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            sut.Settings = null!;
            sut.Context = null!;

            // Act
            sut.Invoking(x => x.ShouldRenderNullablePragmas)
               .Should().Throw<ArgumentNullException>().WithParameterName("Settings");
        }

        [Fact]
        public void Throws_When_Context_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            sut.Settings = CreateCsharpClassGeneratorSettings();
            sut.Context = null!;

            // Act
            sut.Invoking(x => x.ShouldRenderNullablePragmas)
               .Should().Throw<ArgumentNullException>().WithParameterName("Context");
        }

        [Fact]
        public void Returns_False_When_EnableNullableContext_Is_Set_To_False()
        {
            // Arrange
            var sut = CreateSut();
            var settings = CreateCsharpClassGeneratorSettings(enableNullableContext: false);
            sut.Settings = settings;
            sut.Context = CreateTemplateContext();

            // Act
            var result = sut.ShouldRenderNullablePragmas;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_Context_Is_Nested_Type()
        {
            // Arrange
            var sut = CreateSut();
            var settings = CreateCsharpClassGeneratorSettings(enableNullableContext: true);
            sut.Settings = settings;
            var templateContext = Fixture.Create<ITemplateContext>();
            var parentTemplateContext = Fixture.Create<ITemplateContext>();
            templateContext.Model.Returns(new ClassBuilder().WithName("MyClass").Build());
            templateContext.ParentContext.Returns(parentTemplateContext);
            parentTemplateContext.Model.Returns(new ClassBuilder().WithName("MyParentClass").Build());
            parentTemplateContext.ParentContext.Returns(default(ITemplateContext));
            sut.Context = templateContext;

            // Act
            var result = sut.ShouldRenderNullablePragmas;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_When_EnableNullableContext_Is_Set_To_True_And_Context_Is_Not_Nested_Type()
        {
            // Arrange
            var sut = CreateSut();
            var settings = CreateCsharpClassGeneratorSettings(enableNullableContext: true);
            sut.Settings = settings;
            var templateContext = Fixture.Create<ITemplateContext>();
            templateContext.Model.Returns(new ClassBuilder().WithName("MyClass").Build());
            templateContext.ParentContext.Returns(default(ITemplateContext));
            sut.Context = templateContext;

            // Act
            var result = sut.ShouldRenderNullablePragmas;

            // Assert
            result.Should().BeTrue();
        }
    }
}
