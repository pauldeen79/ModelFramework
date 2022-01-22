using System.Diagnostics.CodeAnalysis;
using System.IO;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;
using Xunit;

namespace ModelFramework.CodeGeneration.Tests
{
    [ExcludeFromCodeCoverage]
    public class CodeGenerationTests
    {
        private static readonly CodeGenerationSettings Settings = new CodeGenerationSettings
        (
            basePath: Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\"),
            generateMultipleFiles: false,
            dryRun: true,
            lastGeneratedFilesFileName: "*.generated.cs");

        [Fact]
        public void CanGenerateAll()
        {
            Verify(GenerateCode.For<CommonBuilders>(Settings));
            Verify(GenerateCode.For<ObjectsBuilders>(Settings));
            Verify(GenerateCode.For<ObjectsCodeStatements>(Settings));
            Verify(GenerateCode.For<DatabaseBuilders>(Settings));
            Verify(GenerateCode.For<DatabaseCodeStatements>(Settings));
            Verify(GenerateCode.For<CommonRecords>(Settings));
            Verify(GenerateCode.For<ObjectsRecords>(Settings));
            Verify(GenerateCode.For<DatabaseRecords>(Settings));
        }

        private void Verify(GenerateCode generatedCode)
        {
            if (Settings.DryRun)
            {
                var actual = generatedCode.GenerationEnvironment.ToString();

                // Assert
                actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
            }
        }
    }
}
