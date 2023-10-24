﻿namespace ClassFramework.Pipelines.Tests.Builder;

public class BuilderContextTests : TestBase
{
    public class Constructor : BuilderContextTests
    {
        [Fact]
        public void Throws_On_Null_SourceModel()
        {
            // Act & Assert
            this.Invoking(_ => new BuilderContext(sourceModel: null!, new PipelineBuilderSettings(), CultureInfo.InvariantCulture))
                .Should().Throw<ArgumentNullException>().WithParameterName("sourceModel");
        }

        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Act & Assert
            this.Invoking(_ => new BuilderContext(sourceModel: CreateModel(), settings: null!, CultureInfo.InvariantCulture))
                .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Throws_On_Null_FormatProvider()
        {
            // Act & Assert
            this.Invoking(_ => new BuilderContext(sourceModel: CreateModel(), new PipelineBuilderSettings(), formatProvider: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("formatProvider");
        }
    }

    public class CreatePragmaWarningDisableStatements : BuilderContextTests
    {
        [Fact]
        public void Returns_Empty_Array_When_Pragmas_Are_Not_Needed()
        {
            // Arrange
            var settings = new PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(enableNullableReferenceTypes: false));
            var sut = new BuilderContext(CreateModel(), settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.CreatePragmaWarningDisableStatements();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_When_Pragmas_Are_Needed()
        {
            // Arrange
            var settings = new PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(enableNullableReferenceTypes: true));
            var sut = new BuilderContext(CreateModel(), settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.CreatePragmaWarningDisableStatements();

            // Assert
            result.Should().BeEquivalentTo
            (
                "#pragma warning disable CS8604 // Possible null reference argument.",
                "#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type."
            );
        }
    }

    public class CreatePragmaWarningRestoreStatements : BuilderContextTests
    {
        [Fact]
        public void Returns_Empty_Array_When_Pragmas_Are_Not_Needed()
        {
            // Arrange
            var settings = new PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(enableNullableReferenceTypes: false));
            var sut = new BuilderContext(CreateModel(), settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.CreatePragmaWarningRestoreStatements();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_When_Pragmas_Are_Needed()
        {
            // Arrange
            var settings = new PipelineBuilderSettings(generationSettings: new PipelineBuilderGenerationSettings(enableNullableReferenceTypes: true));
            var sut = new BuilderContext(CreateModel(), settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.CreatePragmaWarningRestoreStatements();

            // Assert
            result.Should().BeEquivalentTo
            (
                "#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.",
                "#pragma warning restore CS8604 // Possible null reference argument."
            );
        }
    }
}