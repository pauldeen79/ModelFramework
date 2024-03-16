namespace DatabaseFramework.TemplateFramework.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;

    public IntegrationTests()
    {
        var templateFactory = Fixture.Freeze<ITemplateFactory>();
        var templateProviderPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        _serviceProvider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkCodeGeneration()
            .AddDatabaseFrameworkTemplates()
            .AddParsers()
            .AddScoped(_ => templateFactory)
            .AddScoped(_ => templateProviderPluginFactory)
            .AddScoped<TestCodeGenerationProvider>()
            //.AddScoped<ImmutableCoreBuilders>()
            .BuildServiceProvider();
        _scope = _serviceProvider.CreateScope();
        templateFactory.Create(Arg.Any<Type>()).Returns(x => _scope.ServiceProvider.GetRequiredService(x.ArgAt<Type>(0)));
    }

    [Fact]
    public void Can_Generate_Code_For_Table()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TestCodeGenerationProvider>();
        var generationEnvironment = new MultipleContentBuilderEnvironment();
        var codeGenerationSettings = new CodeGenerationSettings(string.Empty, "GeneratedCode.sql", dryRun: true);

        // Act
        engine.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

        // Assert
        generationEnvironment.Builder.Contents.Should().ContainSingle();
        generationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"");
    }
    public void Dispose()
    {
        _scope.Dispose();
        _serviceProvider.Dispose();
    }

    private sealed class TestCodeGenerationProvider : DatabaseSchemaGeneratorCodeGenerationProviderBase
    {
        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder()
                .WithName("MyTable")
                .AddFields(new TableFieldBuilder().WithName("MyField").WithType(SqlFieldType.VarChar).WithStringLength(32))
                .Build()
        ];

        public override string Path => string.Empty;
        public override bool RecurseOnDeleteGeneratedFiles => false;
        public override string LastGeneratedFilesFilename => string.Empty;
        public override Encoding Encoding => Encoding.UTF8;

        public override DatabaseSchemaGeneratorSettings Settings => new DatabaseSchemaGeneratorSettingsBuilder()
            .WithPath(Path)
            .WithRecurseOnDeleteGeneratedFiles(RecurseOnDeleteGeneratedFiles)
            .WithLastGeneratedFilesFilename(LastGeneratedFilesFilename)
            .WithEncoding(Encoding)
            .WithCultureInfo(CultureInfo.InvariantCulture)
            .WithCreateCodeGenerationHeader()
            .Build();
    }
}
