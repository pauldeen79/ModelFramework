﻿namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddAttributesFeatureTests : TestBase<Pipelines.Builder.Features.AddAttributesFeature>
{
    public class Process : AddAttributesFeatureTests
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
        public void Adds_Attributes_When_CopyAttributes_Setting_Is_True()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddAttributes(new AttributeBuilder().WithName("MyAttribute")).Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(copyAttributes: true);
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Attributes.Should().BeEquivalentTo(new[] { new AttributeBuilder().WithName("MyAttribute") });
        }

        [Fact]
        public void Does_Not_Add_Attributes_When_CopyAttributes_Setting_Is_False()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddAttributes(new AttributeBuilder().WithName("MyAttribute")).Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(copyAttributes: false);
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Attributes.Should().BeEmpty();
        }
    }
}
