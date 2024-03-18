﻿
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
            .AddScoped<TableCodeGenerationProvider>()
            .BuildServiceProvider();
        _scope = _serviceProvider.CreateScope();
        templateFactory.Create(Arg.Any<Type>()).Returns(x => _scope.ServiceProvider.GetRequiredService(x.ArgAt<Type>(0)));
    }

    [Fact]
    public void Can_Generate_Code_For_Table()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableCodeGenerationProvider>();
        var generationEnvironment = new MultipleContentBuilderEnvironment();
        var codeGenerationSettings = new CodeGenerationSettings(string.Empty, "GeneratedCode.sql", dryRun: true);

        // Act
        engine.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

        // Assert
        generationEnvironment.Builder.Contents.Should().ContainSingle();
        generationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"/****** Object:  Table [dbo].[MyTable] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MyTable](
    MyField varchar(32) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
    }

    public void Dispose()
    {
        _scope.Dispose();
        _serviceProvider.Dispose();
    }

    private abstract class TestCodeGenerationProviderBase : DatabaseSchemaGeneratorCodeGenerationProviderBase
    {
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

    private sealed class TableCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder()
                .WithName("MyTable")
                .AddFields(new TableFieldBuilder().WithName("MyField").WithType(SqlFieldType.VarChar).WithStringLength(32))
                .Build()
        ];
    }
}
