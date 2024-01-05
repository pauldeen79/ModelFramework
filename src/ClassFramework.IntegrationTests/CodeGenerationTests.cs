namespace ClassFramework.IntegrationTests;

public class CodeGenerationTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;
    private readonly IFixture _fixture;
    private readonly Type[] _generationTypes = typeof(CodeGenerationTests).Assembly.GetExportedTypes().Where(x => !x.IsAbstract && x.BaseType == typeof(CsharpClassGeneratorCodeGenerationProviderBase)).ToArray();

    public CodeGenerationTests()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        var templateFactory = _fixture.Freeze<ITemplateFactory>();
        var templateProviderPluginFactory = _fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkCodeGeneration()
            .AddCsharpExpressionCreator()
            .AddClassFrameworkTemplates()
            .AddScoped(_ => templateFactory)
            .AddScoped(_ => templateProviderPluginFactory);
        
        foreach (var type in _generationTypes)
        {
            services.AddScoped(type);
        }
        
        _serviceProvider = services.BuildServiceProvider();
        _scope = _serviceProvider.CreateScope();
        templateFactory.Create(Arg.Any<Type>()).Returns(x => _scope.ServiceProvider.GetRequiredService(x.ArgAt<Type>(0)));
    }

    [Fact]
    public void Can_Generate_Code()
    {
        // Arrange
        var generationEnvironment = new MultipleContentBuilderEnvironment();
        var instances = _generationTypes
            .Select(x => (CsharpClassGeneratorCodeGenerationProviderBase)_scope.ServiceProvider.GetRequiredService(x))
            .ToArray();
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationSettings = new CodeGenerationSettings(string.Empty, "GeneratedCode.cs", dryRun: true);

        // Act
        foreach (var instance in instances)
        {
            engine.Generate(instance, generationEnvironment, codeGenerationSettings);
        }

        // Assert
        generationEnvironment.Builder.Contents.Should().NotBeEmpty();
    }
}
