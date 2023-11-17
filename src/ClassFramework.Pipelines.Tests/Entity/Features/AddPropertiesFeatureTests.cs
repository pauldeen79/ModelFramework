namespace ClassFramework.Pipelines.Tests.Entity.Features;

public class AddPropertiesFeatureTests : TestBase<Pipelines.Entity.Features.AddPropertiesFeature>
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
        public void Adds_Properties_From_SourceModel()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings();
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2", "Property3");
        }

        [Fact]
        public void Maps_TypeNames_Correctly()
        {
        }

        [Fact]
        public void Adds_Setters_When_Specified_In_Settings()
        {
        }

        [Fact]
        public void Sets_SetterVisibility_From_Settings()
        {
        }

        [Fact]
        public void Adds_Mapped_And_Filtered_Attributes_According_To_Settings()
        {
        }
    }
}
