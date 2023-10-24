﻿namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class GenericsFeatureTests : TestBase<GenericsFeature>
{
    public class Process : GenericsFeatureTests
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
        public void Adds_GenericTypeArguments()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddGenericTypeArguments("T").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new Pipelines.Builder.PipelineBuilderSettings(classSettings: new Entity.PipelineBuilderSettings(constructorSettings: new Entity.PipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.Shared)));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.GenericTypeArguments.Should().BeEquivalentTo("T");
        }

        [Fact]
        public void Adds_GenericTypeArgumentConstraints()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddGenericTypeArguments("T").AddGenericTypeArgumentConstraints("where T : class").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new Pipelines.Builder.PipelineBuilderSettings(classSettings: new Entity.PipelineBuilderSettings(constructorSettings: new Entity.PipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.Shared)));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.GenericTypeArgumentConstraints.Should().BeEquivalentTo("where T : class");
        }
    }
}
