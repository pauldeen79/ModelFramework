namespace ClassFramework.Pipelines.Tests.Reflection;

public class ReflectionContextTests : TestBase
{
    public class Constructor : ReflectionContextTests
    {
        [Fact]
        public void Throws_On_Null_SourceModel()
        {
            // Act & Assert
            this.Invoking(_ => new ReflectionContext(sourceModel: null!, new Pipelines.Reflection.PipelineBuilderSettings(), CultureInfo.InvariantCulture))
                .Should().Throw<ArgumentNullException>().WithParameterName("sourceModel");
        }

        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Act & Assert
            this.Invoking(_ => new ReflectionContext(sourceModel: GetType(), settings: null!, CultureInfo.InvariantCulture))
                .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Throws_On_Null_FormatProvider()
        {
            // Act & Assert
            this.Invoking(_ => new ReflectionContext(sourceModel: GetType(), new Pipelines.Reflection.PipelineBuilderSettings(), formatProvider: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("formatProvider");
        }
    }
}
