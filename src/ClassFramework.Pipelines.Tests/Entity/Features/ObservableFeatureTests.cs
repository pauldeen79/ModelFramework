﻿namespace ClassFramework.Pipelines.Tests.Entity.Features;

public class ObservableFeatureTests : TestBase<Pipelines.Entity.Features.ObservableFeature>
{
    public class Process : ObservableFeatureTests
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
        public void Does_Not_Add_Interface_And_Event_When_CreateAsObservable_Is_False()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(createAsObservable: false);
            var context = new PipelineContext<IConcreteTypeBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().BeEmpty();
            model.Fields.Should().BeEmpty();
        }

        [Fact]
        public void Adds_Interface_And_Event_When_CreateAsObservable_Is_True()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(createAsObservable: true);
            var context = new PipelineContext<IConcreteTypeBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Interfaces.Should().BeEquivalentTo("System.ComponentModel.INotifyPropertyChanged");
            model.Fields.Select(x => x.Name).Should().BeEquivalentTo("PropertyChanged");
            model.Fields.Select(x => x.TypeName).Should().BeEquivalentTo("System.ComponentModel.PropertyChangedEventHandler");
            model.Fields.Select(x => x.Event).Should().BeEquivalentTo(new[] { true });
            model.Fields.Select(x => x.Visibility).Should().BeEquivalentTo(new[] { Visibility.Public });
            model.Fields.Select(x => x.IsNullable).Should().BeEquivalentTo(new[] { true });
        }
    }
}
