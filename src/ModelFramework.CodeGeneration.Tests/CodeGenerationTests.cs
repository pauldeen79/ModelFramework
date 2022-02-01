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
        // Arrange
        var models = new[]
        {
            typeof(IAttribute),
            typeof(IAttributeParameter),
            typeof(IClass),
            typeof(IClassConstructor),
            typeof(IClassField),
            typeof(IClassMethod),
            typeof(IClassProperty),
            typeof(IEnum),
            typeof(IEnumMember),
            typeof(IInterface),
            typeof(IParameter)
        }.Select(x => x.ToClass(new ClassSettings()).ToInterfaceBuilder()).ToArray();
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection
            .AddCsharpExpressionDumper()
            .AddSingleton<IObjectHandlerPropertyFilter, SkipDefaultValuesForModelFramework>()
            .BuildServiceProvider();
        var dumper = serviceProvider.GetRequiredService<ICsharpExpressionDumper>();

        // Act
        var code = dumper.Dump(models);

        // Assert
        code.Should().NotBeEmpty();
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
