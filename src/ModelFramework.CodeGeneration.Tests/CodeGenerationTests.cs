namespace ModelFramework.CodeGeneration.Tests;

public class CodeGenerationTests
{
    private static readonly CodeGenerationSettings Settings = new CodeGenerationSettings
    (
        basePath: Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\"),
        generateMultipleFiles: false,
        dryRun: true
    );

    // Bootstrap test that generates c# code for the model used in code generation :)
    [Fact]
    public void Can_Generate_Model_For_Abstractions()
    {
        // Act & Assert
        Verify(GenerateCode.For<AbstractionsInterfacesModels>(Settings));
    }

    [Fact]
    public void Can_Generate_All_Classes_For_ModelFramework()
    {
        // Act & Assert
        Verify(GenerateCode.For<CommonBuilders>(Settings));
        Verify(GenerateCode.For<ObjectsBuilders>(Settings));
        Verify(GenerateCode.For<ObjectsCodeStatements>(Settings));
        Verify(GenerateCode.For<DatabaseBuilders>(Settings));
        Verify(GenerateCode.For<DatabaseCodeStatements>(Settings));
        Verify(GenerateCode.For<CommonRecords>(Settings));
        Verify(GenerateCode.For<ObjectsRecords>(Settings));
        Verify(GenerateCode.For<DatabaseRecords>(Settings));
    }

    // Example how to generate builder extensions
    [Fact]
    public void Can_Generate_Builder_Extensions_For_ModelFramework()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<CommonBuildersExtensions>(settings);
        var actual = generatedCode.GenerationEnvironment.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
    }

    private void Verify(GenerateCode generatedCode)
    {
        if (Settings.DryRun)
        {
            // Act
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }
    }
}
