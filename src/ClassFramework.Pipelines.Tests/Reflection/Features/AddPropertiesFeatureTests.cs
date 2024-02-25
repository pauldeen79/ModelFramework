﻿namespace ClassFramework.Pipelines.Tests.Reflection.Features;

public class AddPropertiesFeatureTests : TestBase<Pipelines.Reflection.Features.AddPropertiesFeature>
{
    public class Process : AddPropertiesFeatureTests
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
        public void Copies_Properties()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyClass);
            var model = new ClassBuilder();
            var settings = CreateSettingsForReflection();
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings.Build(), CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Should().ContainSingle();
        }
    }
}
