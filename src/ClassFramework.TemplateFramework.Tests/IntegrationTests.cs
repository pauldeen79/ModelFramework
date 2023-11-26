using NSubstitute;

namespace ClassFramework.TemplateFramework.Tests;

public class IntegrationTests : TestBase
{
    [Fact]
    public void Generation_Just_Works()
    {
        // Arrange
        //var fileSystem = Fixture.Freeze<IFileSystem>();
        var templateFactory = Fixture.Freeze<ITemplateFactory>();
        var templateProviderPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        using var serviceProvider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkCodeGeneration()
            //.AddScoped(_ => fileSystem)
            .AddScoped(_ => templateFactory)
            .AddScoped(_ => templateProviderPluginFactory)
            .BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var engine = scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = new TestCodeGenerationProvider(
            new[] { new ClassBuilder().WithName("MyClass").Build() },
            string.Empty,
            false,
            string.Empty,
            Encoding.UTF8,
            true,
            false,
            true,
            true,
            CultureInfo.InvariantCulture
        );
        var generationEnvironment = new MultipleContentBuilderEnvironment();
        var settings = new CodeGenerationSettings(string.Empty, "GeneratedCode.cs", dryRun: true);
        templateFactory.Create(Arg.Any<Type>()).Returns(x => Activator.CreateInstance(x.ArgAt<Type>(0))!);

        // Act
        engine.Generate(codeGenerationProvider, generationEnvironment, settings);

        // Assert
        generationEnvironment.Builder.Contents.Should().ContainSingle();
    }

    private sealed class TestCodeGenerationProvider : CsharpClassGeneratorCodeGenerationProviderBase
    {
        public TestCodeGenerationProvider(IEnumerable<TypeBase> model,
                                          string path,
                                          bool recurseOnDeleteGeneratedFiles,
                                          string lastGeneratedFilesFilename,
                                          Encoding encoding,
                                          bool generateMultipleFiles,
                                          bool skipWhenFileExists,
                                          bool createCodeGenerationHeader,
                                          bool enableNullableContext,
                                          CultureInfo cultureInfo,
                                          string filenameSuffix = ".template.generated",
                                          string? environmentVersion = null)
            : base(
                  model,
                  path,
                  recurseOnDeleteGeneratedFiles,
                  lastGeneratedFilesFilename,
                  encoding,
                  generateMultipleFiles,
                  skipWhenFileExists,
                  createCodeGenerationHeader,
                  enableNullableContext,
                  cultureInfo,
                  filenameSuffix,
                  environmentVersion)
        {
        }
    }
}
