using System.Diagnostics.CodeAnalysis;
using System.IO;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;
using ModelFramework.CodeGeneration.Tests.Helpers;
using Xunit;

namespace ModelFramework.CodeGeneration.Tests
{
    [ExcludeFromCodeCoverage]
    public class CodeGenerationTests
    {
        private static readonly string BasePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\");
        private const bool GenerateMultipleFiles = false;
        private const bool DryRun = true;
        private const string LastGeneratedFilesFileName = "*.generated.cs;*.generated.sql";

        [Fact]
        public void CanGenerateAll()
        {
            GenerateCode.For<CommonBuilders>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            GenerateCode.For<ObjectsBuilders>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            GenerateCode.For<ObjectsCodeStatements>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            GenerateCode.For<DatabaseBuilders>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            GenerateCode.For<DatabaseCodeStatements>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            GenerateCode.For<CommonRecords>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            GenerateCode.For<ObjectsRecords>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            GenerateCode.For<DatabaseRecords>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForCommonContracts()
        {
            // Act
            var generatedCode = GenerateCode.For<CommonBuilders>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForObjectsContracts()
        {
            // Act
            var generatedCode = GenerateCode.For<ObjectsBuilders>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForObjectsCodeStatements()
        {
            // Act
            var generatedCode = GenerateCode.For<ObjectsCodeStatements>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForDatabaseContracts()
        {
            // Act
            var generatedCode = GenerateCode.For<DatabaseBuilders>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForDatabaseCodeStatements()
        {
            // Act
            var generatedCode = GenerateCode.For<DatabaseCodeStatements>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateRecordsForCommonContracts()
        {
            // Act
            var generatedCode = GenerateCode.For<CommonRecords>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateRecordsForObjectsContracts()
        {
            // Act
            var generatedCode = GenerateCode.For<ObjectsRecords>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateRecordsForDatabaseContracts()
        {
            // Act
            var generatedCode = GenerateCode.For<DatabaseRecords>(BasePath, GenerateMultipleFiles, DryRun, LastGeneratedFilesFileName);
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }
    }
}
