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
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Attributes.Should().BeEquivalentTo(new[] { new AttributeBuilder().WithName("MyAttribute") });
        }

        [Fact]
        public void Adds_Filtered_Attributes_When_CopyAttributes_Setting_Is_True_And_Predicate_Is_Filled()
        {
            // Arrange
            var sourceModel = new ClassBuilder()
                .WithName("SomeClass")
                .WithNamespace("SomeNamespace")
                .AddAttributes(
                    new AttributeBuilder().WithName("MyAttribute1"),
                    new AttributeBuilder().WithName("MyAttribute2"))
                .Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(copyAttributes: true, copyAttributePredicate: x => x.Name == "MyAttribute2");
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Attributes.Should().BeEquivalentTo(new[] { new AttributeBuilder().WithName("MyAttribute2") });
        }

        [Fact]
        public void Does_Not_Add_Attributes_When_CopyAttributes_Setting_Is_False()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddAttributes(new AttributeBuilder().WithName("MyAttribute")).Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(copyAttributes: false);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Attributes.Should().BeEmpty();
        }

        private static PipelineContext<IConcreteTypeBuilder, BuilderContext> CreateContext(TypeBase sourceModel, ClassBuilder model, Pipelines.Builder.PipelineSettings settings)
            => new(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));
    }
}
