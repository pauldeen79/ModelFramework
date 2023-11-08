﻿namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddInterfacesFeatureTests : TestBase<Pipelines.Builder.Features.AddInterfacesFeature>
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
        public void Adds_Interfaces_When_CopyInterfaces_Setting_Is_True()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddInterfaces("IMyInterface").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(copyInterfaces: true);
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().BeEquivalentTo("IMyInterface");
        }

        [Fact]
        public void Adds_Filtered_Interfaces_When_CopyInterfaces_Setting_Is_True_And_Predicate_Is_Filled()
        {
            // Arrange
            var sourceModel = new ClassBuilder()
                .WithName("SomeClass")
                .WithNamespace("SomeNamespace")
                .AddInterfaces(
                    "IMyInterface1",
                    "IMyInterface2")
                .Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(copyInterfaces: true, copyInterfacePredicate: x => x == "IMyInterface2");
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().BeEquivalentTo("IMyInterface2");
        }

        [Fact]
        public void Does_Not_Add_Interfaces_When_CopyInterfaces_Setting_Is_False()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddInterfaces("IMyInterface").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(copyInterfaces: false);
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().BeEmpty();
        }
    }
}
