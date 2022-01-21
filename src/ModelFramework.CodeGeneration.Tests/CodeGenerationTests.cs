using System.Diagnostics.CodeAnalysis;
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
        [Fact]
        public void CanGenerateImmutableBuilderClassesForCommonContracts()
        {
            // Act
            var actual = GenerateCode.For<CommonBuilders>();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForObjectsContracts()
        {
            // Act
            var actual = GenerateCode.For<ObjectsBuilders>();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForObjectsCodeStatements()
        {
            // Act
            var actual = GenerateCode.For<ObjectsCodeStatements>();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForDatabaseContracts()
        {
            // Act
            var actual = GenerateCode.For<DatabaseBuilders>();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateImmutableBuilderClassesForDatabaseCodeStatements()
        {
            // Act
            var actual = GenerateCode.For<DatabaseCodeStatements>();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateRecordsForCommonContracts()
        {
            // Act
            var actual = GenerateCode.For<CommonRecords>();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateRecordsForObjectsContracts()
        {
            // Act
            var actual = GenerateCode.For<ObjectsRecords>();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }

        [Fact]
        public void CanGenerateRecordsForDatabaseContracts()
        {
            // Act
            var actual = GenerateCode.For<DatabaseRecords>();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }
    }
}
