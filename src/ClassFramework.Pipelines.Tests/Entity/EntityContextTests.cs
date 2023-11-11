namespace ClassFramework.Pipelines.Tests.Entity;

public class EntityContextTests : TestBase
{
    public class Constructor : EntityContextTests
    {
        [Fact]
        public void Throws_On_Null_Model()
        {
            // Act & Assert
            this.Invoking(_ => new EntityContext(model: null!, new Pipelines.Entity.PipelineBuilderSettings(), CultureInfo.InvariantCulture))
                .Should().Throw<ArgumentNullException>().WithParameterName("model");
        }

        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Act & Assert
            this.Invoking(_ => new EntityContext(model: CreateModel(), settings: null!, CultureInfo.InvariantCulture))
                .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Throws_On_Null_FormatProvider()
        {
            // Act & Assert
            this.Invoking(_ => new EntityContext(model: CreateModel(), new Pipelines.Entity.PipelineBuilderSettings(), formatProvider: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("formatProvider");
        }
    }
}
