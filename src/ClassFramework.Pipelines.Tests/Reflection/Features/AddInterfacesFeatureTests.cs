﻿namespace ClassFramework.Pipelines.Tests.Reflection.Features;

public class AddInterfacesFeatureTests : TestBase<Pipelines.Reflection.Features.AddInterfacesFeature>
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
        public void Does_Not_Copy_Interfaces_When_CopyInterfaces_Is_False()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyClass);
            var model = new ClassBuilder();
            var settings = CreateReflectionSettings(copyInterfaces: false);
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().BeEmpty();
        }

        [Fact]
        public void Copies_Interfaces_When_CopyInterfaces_Is_True()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyClass);
            var model = new ClassBuilder();
            var settings = CreateReflectionSettings(copyInterfaces: true);
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().ContainSingle();
        }

        [Fact]
        public void Filters_Interfaces_When_CopyInterfaces_Is_True_And_Predicate_Is_Set()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyClass);
            var model = new ClassBuilder();
            var settings = CreateReflectionSettings(copyInterfaces: false, copyInterfacePredicate: _ => false);
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().BeEmpty();
        }
    }
}
