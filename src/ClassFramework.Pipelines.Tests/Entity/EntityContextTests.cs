using ClassFramework.Pipelines.Tests.Builder;

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

    public class MapTypeName : BuilderContextTests
    {
        [Fact]
        public void Throws_On_Null_TypeName()
        {
            // Arrange
            var settings = CreateBuilderSettings(enableNullableReferenceTypes: false);
            var sut = new BuilderContext(CreateModel(), settings, CultureInfo.InvariantCulture);

            // Act & Assert
            sut.Invoking(x => x.MapTypeName(typeName: null!))
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("typeName");
        }
    }

    public class MapAttribute : BuilderContextTests
    {
        [Fact]
        public void Throws_On_Null_TypeName()
        {
            // Arrange
            var settings = CreateBuilderSettings(enableNullableReferenceTypes: false);
            var sut = new BuilderContext(CreateModel(), settings, CultureInfo.InvariantCulture);

            // Act & Assert
            sut.Invoking(x => x.MapAttribute(attribute: null!))
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("attribute");
        }
    }
}
