﻿namespace ClassFramework.TemplateFramework.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;

    public IntegrationTests()
    {
        var templateFactory = Fixture.Freeze<ITemplateFactory>();
        templateFactory.Create(Arg.Any<Type>()).Returns(x => Activator.CreateInstance(x.ArgAt<Type>(0))!);
        var templateProviderPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        _serviceProvider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkCodeGeneration()
            .AddScoped(_ => templateFactory)
            .AddScoped(_ => templateProviderPluginFactory)
            .BuildServiceProvider();
        _scope = _serviceProvider.CreateScope();
    }

    [Fact]
    public void Generation_Just_Works()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = new TestCodeGenerationProvider(
            new[] { new ClassBuilder().WithNamespace("MyNamespace").WithName("MyClass").Build() },
            string.Empty,
            false,
            string.Empty,
            Encoding.UTF8,
            true,
            false,
            true,
            true,
            CultureInfo.InvariantCulture,
            environmentVersion: "1.0.0"
        );
        var generationEnvironment = new MultipleContentBuilderEnvironment();
        var settings = new CodeGenerationSettings(string.Empty, "GeneratedCode.cs", dryRun: true);

        // Act
        engine.Generate(codeGenerationProvider, generationEnvironment, settings);

        // Assert
        generationEnvironment.Builder.Contents.Should().ContainSingle();
        generationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 1.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace
{
#nullable enable
    public class MyClass
    {
    }
#nullable restore
}
");
    }

    public void Dispose()
    {
        _scope.Dispose();
        _serviceProvider.Dispose();
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
