namespace ClassFramework.Pipelines.Tests.Entity;

public class PipelineBuilderTests : IntegrationTestBase<IPipelineBuilder<ClassBuilder, EntityContext>>
{
    public class Process : PipelineBuilderTests
    {
        private EntityContext CreateContext(bool addProperties = true) => new EntityContext
        (
            CreateGenericModel(addProperties),
            CreateEntitySettings
            (
                allowGenerationWithoutProperties: false
            ),
            CultureInfo.InvariantCulture
        );

        private ClassBuilder Model { get; } = new();

        [Fact]
        public void Sets_Partial()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().NotBeNull();
            result.Value!.Partial.Should().BeTrue();
        }

        [Fact]
        public void Sets_Namespace_And_Name_According_To_Settings()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().NotBeNull();
            result.Value!.Name.Should().Be("MyClass");
            result.Value.Namespace.Should().Be("MyNamespace");
        }

        [Fact]
        public void Returns_Invalid_When_SourceModel_Does_Not_Have_Properties_And_AllowGenerationWithoutProperties_Is_False()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext(addProperties: false));

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("To create a builder class, there must be at least one property");
        }
    }
}
