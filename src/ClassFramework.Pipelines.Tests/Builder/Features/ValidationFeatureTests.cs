﻿namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class ValidationFeatureTests : TestBase<ValidationFeature>
{
    public class Process : ValidationFeatureTests
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
        public void Returns_Continue_When_Properties_Are_Found()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new Pipelines.Builder.PipelineBuilderSettings();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Continue_When_Properties_Are_Not_Found_But_AllowGenerationWithoutProperties_Is_True()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("MyClass").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new Pipelines.Builder.PipelineBuilderSettings(classSettings: new Pipelines.Entity.PipelineBuilderSettings(allowGenerationWithoutProperties: true));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Continue_When_Properties_Are_Not_Found_But_EnableInheritance_Is_True()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("MyClass").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new Pipelines.Builder.PipelineBuilderSettings(classSettings: new Pipelines.Entity.PipelineBuilderSettings(allowGenerationWithoutProperties: false, inheritanceSettings: new Pipelines.Entity.PipelineBuilderInheritanceSettings(enableInheritance: true)));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }
    }
}
