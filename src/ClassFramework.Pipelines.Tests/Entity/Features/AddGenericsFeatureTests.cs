﻿namespace ClassFramework.Pipelines.Tests.Entity.Features;

public class AddGenericsFeatureTests : TestBase<Pipelines.Entity.Features.AddGenericsFeature>
{
    public class Process : AddGenericsFeatureTests
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
        public void Adds_Generics_When_Available()
        {
            // Arrange
            var sourceModel = CreateGenericModel(addProperties: false);
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings();
            var context = new PipelineContext<IConcreteTypeBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.GenericTypeArguments.Should().BeEquivalentTo("T");
            model.GenericTypeArgumentConstraints.Should().BeEquivalentTo("where T : class");
        }
    }
}
