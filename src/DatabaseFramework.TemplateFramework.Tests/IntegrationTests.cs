namespace DatabaseFramework.TemplateFramework.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;

    private MultipleContentBuilderEnvironment GenerationEnvironment { get; } = new MultipleContentBuilderEnvironment();
    private CodeGenerationSettings CodeGenerationSettings { get; } = new CodeGenerationSettings(string.Empty, "GeneratedCode.sql", dryRun: true);

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
            .AddScoped<TablesCodeGenerationProvider>()
            .AddScoped<TableWithVarcharAndNumericFieldsCodeGenerationProvider>()
            .AddScoped<TableWithCheckConstraintsOnFieldLevelCodeGenerationProvider>()
            .AddScoped<TableWithCheckConstraintsOnTableLevelCodeGenerationProvider>()
            .AddScoped<TableWithIndexesCodeGenerationProvider>()
            .AddScoped<TableWithPrimaryKeyConstraintCodeGenerationProvider>()
            .AddScoped<TableWithUniqueConstraintCodeGenerationProvider>()
            .AddScoped<TableWithDefaultValueConstraintCodeGenerationProvider>()
            .AddScoped<StoredProcedureContainingStatementsCodeGenerationProvider>()
            .AddScoped<TableWithForeignKeyConstraintCodeGenerationProvider>()
            .AddScoped<TableWithCascadeForeignKeyConstraintCodeGenerationProvider>()
            .AddScoped<TableWithIdentityFieldCodeGenerationProvider>()
            .AddScoped<ViewCodeGenerationProvider>()
            .AddScoped<MultipleFilesCodeGenerationProvider>()
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

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"/****** Object:  Table [dbo].[MyTable] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MyTable](
	[MyField] VARCHAR(32) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_Tables()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TablesCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"/****** Object:  Table [dbo].[Table1] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field] INT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Table2] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table2](
	[Field] INT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Table3] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table3](
	[Field] INT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithFields()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithVarcharAndNumericFieldsCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL,
	[Field2] VARCHAR(32) COLLATE Latin1_General_CI_AS NULL,
	[Field3] NUMERIC(8,2) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithCheckConstraintOnFieldLevel()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithCheckConstraintsOnFieldLevelCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL
    CONSTRAINT [CHK1]
    CHECK ([Field1] BETWEEN 1 AND 10),
	[Field2] VARCHAR(32) NULL,
	[Field3] NUMERIC(8,2) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithCheckConstraintOnTableLevel()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithCheckConstraintsOnTableLevelCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL,
	[Field2] INT NULL,
    CONSTRAINT [MyCheckConstraint1]
    CHECK (Field1 > 10),
    CONSTRAINT [MyCheckConstraint2]
    CHECK (Field2 > 20)
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithIndexes()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithIndexesCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Index1] ON [dbo].[Table1]
(
	[Field1] ASC,
	[Field2] DESC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Index2] ON [dbo].[Table1]
(
	[Field1] ASC,
	[Field2] DESC
) ON [PRIMARY]
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithPrimaryKeyConstraint()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithPrimaryKeyConstraintCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL,
	[Field2] VARCHAR(32) NULL,
	[Field3] NUMERIC(8,2) NOT NULL,
 CONSTRAINT [PK] PRIMARY KEY CLUSTERED
(
	[Field1] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithUniqueConstraint()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithUniqueConstraintCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"/****** Object:  Table [dbo].[Table1] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL,
	[Field2] VARCHAR(32) NULL,
	[Field3] NUMERIC(8,2) NOT NULL,
 CONSTRAINT [UC] UNIQUE NONCLUSTERED
(
	[Field1]
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithDefaultValueConstraint()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithDefaultValueConstraintCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL,
	[Field2] VARCHAR(32) NULL,
	[Field3] NUMERIC(8,2) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [Table1] ADD CONSTRAINT [DVC] DEFAULT (2) FOR [Field1]
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_StoredProcedureContainingStatements()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<StoredProcedureContainingStatementsCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Test]
	@Param1 INT,
	@Param2 INT = 5
AS
BEGIN
    --statement 1 goes here
    --statement 2 goes here
END
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithForeignKeyConstraint()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithForeignKeyConstraintCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL,
	[Field2] VARCHAR(32) NULL,
	[Field3] NUMERIC(8,2) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Table1]  WITH CHECK ADD  CONSTRAINT [FK] FOREIGN KEY([LocalField1],[LocalField2])
REFERENCES [dbo].[ForeignTable] ([RemoteField1],[RemoteField2])
ON UPDATE NO ACTION
ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[Table1] CHECK CONSTRAINT [FK]
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithCascadeForeignKeyConstraint()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithCascadeForeignKeyConstraintCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"/****** Object:  Table [dbo].[Table1] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL,
	[Field2] VARCHAR(32) NULL,
	[Field3] NUMERIC(8,2) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK] ******/
ALTER TABLE [dbo].[Table1]  WITH CHECK ADD  CONSTRAINT [FK] FOREIGN KEY([LocalField1],[LocalField2])
REFERENCES [dbo].[ForeignTable] ([RemoteField1],[RemoteField2])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Table1] CHECK CONSTRAINT [FK]
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_TableWithIdentityField()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<TableWithIdentityFieldCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT IDENTITY(1, 1) NULL,
	[Field2] VARCHAR(32) NULL,
	[Field3] NUMERIC(8,2) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
    }

    [Fact]
    public void Can_Generate_Code_For_Views()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<ViewCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().ContainSingle();
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View1]
AS
SELECT
    [Field1],
    [Field2]
FROM
    [Table1]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View2]
AS
SELECT TOP 50 PERCENT
    [Field2] AS [Alias2]
FROM
    [Table1],
    [Table2]
WHERE
    table1.Field1 = 'Value 1'
    AND table1.Field1 = 'Value 2'
GROUP BY
    [Field1],
    [Field2]
ORDER BY
    [table1.Field1] DESC,
    [table1.Field2] ASC
GO
");
    }

    [Fact]
    public void Can_Generate_Code_To_Different_Files()
    {
        // Arrange
        var engine = _scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = _scope.ServiceProvider.GetRequiredService<MultipleFilesCodeGenerationProvider>();

        // Act
        engine.Generate(codeGenerationProvider, GenerationEnvironment, CodeGenerationSettings);

        // Assert
        GenerationEnvironment.Builder.Contents.Should().HaveCount(3);
        GenerationEnvironment.Builder.Contents.First().Builder.ToString().Should().Be(@"/****** Object:  Table [dbo].[MyTable] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MyTable](
	[MyField] VARCHAR(32) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
        GenerationEnvironment.Builder.Contents.ElementAt(1).Builder.ToString().Should().Be(@"/****** Object:  Stored procedure [dbo].[usp_Test] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Test]
	@Param1 INT,
	@Param2 INT = 5
AS
BEGIN
    --statement 1 goes here
    --statement 2 goes here
END
GO
");
        GenerationEnvironment.Builder.Contents.Last().Builder.ToString().Should().Be(@"/****** Object:  View [dbo].[View1] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View1]
AS
SELECT
    [Field1],
    [Field2]
FROM
    [Table1]
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

    private sealed class TablesCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields(new TableFieldBuilder().WithName("Field").WithType(SqlFieldType.Int)).Build(),
            new TableBuilder().WithName("Table2").AddFields(new TableFieldBuilder().WithName("Field").WithType(SqlFieldType.Int)).Build(),
            new TableBuilder().WithName("Table3").AddFields(new TableFieldBuilder().WithName("Field").WithType(SqlFieldType.Int)).Build()
        ];
    }

    private sealed class TableWithVarcharAndNumericFieldsCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int),
                new TableFieldBuilder().WithName("Field2").WithType(SqlFieldType.VarChar).WithStringLength(32).WithStringCollation("Latin1_General_CI_AS"),
                new TableFieldBuilder().WithName("Field3").WithType(SqlFieldType.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
            ).Build()
        ];
    }

    private sealed class TableWithCheckConstraintsOnFieldLevelCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int).AddCheckConstraints(new CheckConstraintBuilder().WithName("CHK1").WithExpression("[Field1] BETWEEN 1 AND 10")),
                new TableFieldBuilder().WithName("Field2").WithType(SqlFieldType.VarChar).WithStringLength(32),
                new TableFieldBuilder().WithName("Field3").WithType(SqlFieldType.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
            ).Build()
        ];
    }

    private sealed class TableWithCheckConstraintsOnTableLevelCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int),
                new TableFieldBuilder().WithName("Field2").WithType(SqlFieldType.Int)
            ).AddCheckConstraints
            (
                new CheckConstraintBuilder().WithName("MyCheckConstraint1").WithExpression("Field1 > 10"),
                new CheckConstraintBuilder().WithName("MyCheckConstraint2").WithExpression("Field2 > 20")
            ).Build()
        ];
    }

    private sealed class TableWithIndexesCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int)
            ).AddIndexes
            (
                new IndexBuilder().WithName("IX_Index1").WithUnique().AddFields
                (
                    new IndexFieldBuilder().WithName("Field1"),
                    new IndexFieldBuilder().WithName("Field2").WithIsDescending()
                ),
                new IndexBuilder().WithName("IX_Index2").AddFields
                (
                    new IndexFieldBuilder().WithName("Field1"),
                    new IndexFieldBuilder().WithName("Field2").WithIsDescending()
                )
            ).Build()
        ];
    }

    private sealed class TableWithPrimaryKeyConstraintCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int),
                new TableFieldBuilder().WithName("Field2").WithType(SqlFieldType.VarChar).WithStringLength(32),
                new TableFieldBuilder().WithName("Field3").WithType(SqlFieldType.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
            ).AddPrimaryKeyConstraints
            (
                new PrimaryKeyConstraintBuilder().WithName("PK").AddFields
                (
                    new PrimaryKeyConstraintFieldBuilder().WithName("Field1")
                )
            ).Build()
        ];
    }

    private sealed class TableWithUniqueConstraintCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int),
                new TableFieldBuilder().WithName("Field2").WithType(SqlFieldType.VarChar).WithStringLength(32),
                new TableFieldBuilder().WithName("Field3").WithType(SqlFieldType.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
            ).AddUniqueConstraints
            (
                new UniqueConstraintBuilder().WithName("UC").AddFields
                (
                    new UniqueConstraintFieldBuilder().WithName("Field1")
                )
            ).Build()
        ];
    }

    private sealed class TableWithDefaultValueConstraintCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int),
                new TableFieldBuilder().WithName("Field2").WithType(SqlFieldType.VarChar).WithStringLength(32),
                new TableFieldBuilder().WithName("Field3").WithType(SqlFieldType.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
            ).AddDefaultValueConstraints
            (
                new DefaultValueConstraintBuilder().WithFieldName("Field1").WithDefaultValue("2").WithName("DVC")
            ).Build()
        ];
    }

    private sealed class StoredProcedureContainingStatementsCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new StoredProcedureBuilder().WithName("usp_Test").AddParameters
            (
                new StoredProcedureParameterBuilder().WithName("Param1").WithType(SqlFieldType.Int),
                new StoredProcedureParameterBuilder().WithName("Param2").WithType(SqlFieldType.Int).WithDefaultValue("5")
            ).AddStatements
            (
                new StringSqlStatementBuilder().WithStatement("--statement 1 goes here"),
                new StringSqlStatementBuilder().WithStatement("--statement 2 goes here")
            ).Build()
        ];
    }

    private sealed class TableWithForeignKeyConstraintCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int),
                new TableFieldBuilder().WithName("Field2").WithType(SqlFieldType.VarChar).WithStringLength(32),
                new TableFieldBuilder().WithName("Field3").WithType(SqlFieldType.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
            ).AddForeignKeyConstraints
            (
                new ForeignKeyConstraintBuilder().WithName("FK").WithForeignTableName("ForeignTable").AddLocalFields
                (
                    new ForeignKeyConstraintFieldBuilder().WithName("LocalField1"),
                    new ForeignKeyConstraintFieldBuilder().WithName("LocalField2")
                ).AddForeignFields
                (
                    new ForeignKeyConstraintFieldBuilder().WithName("RemoteField1"),
                    new ForeignKeyConstraintFieldBuilder().WithName("RemoteField2")
                )
            ).Build()
        ];
    }

    private sealed class TableWithCascadeForeignKeyConstraintCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int),
                new TableFieldBuilder().WithName("Field2").WithType(SqlFieldType.VarChar).WithStringLength(32),
                new TableFieldBuilder().WithName("Field3").WithType(SqlFieldType.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
            ).AddForeignKeyConstraints
            (
                new ForeignKeyConstraintBuilder().WithName("FK").WithForeignTableName("ForeignTable").AddLocalFields
                (
                    new ForeignKeyConstraintFieldBuilder().WithName("LocalField1"),
                    new ForeignKeyConstraintFieldBuilder().WithName("LocalField2")
                ).AddForeignFields
                (
                    new ForeignKeyConstraintFieldBuilder().WithName("RemoteField1"),
                    new ForeignKeyConstraintFieldBuilder().WithName("RemoteField2")
                ).WithCascadeUpdate(CascadeAction.Cascade).WithCascadeDelete(CascadeAction.Cascade)
            ).Build()
        ];
    }

    private sealed class TableWithIdentityFieldCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder().WithName("Table1").AddFields
            (
                new TableFieldBuilder().WithName("Field1").WithType(SqlFieldType.Int).WithIsIdentity(),
                new TableFieldBuilder().WithName("Field2").WithType(SqlFieldType.VarChar).WithStringLength(32),
                new TableFieldBuilder().WithName("Field3").WithType(SqlFieldType.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
            ).Build()
        ];
    }

    private sealed class ViewCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithCreateCodeGenerationHeader(false).Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new ViewBuilder().WithName("View1")
                .WithDefinition(@"SELECT
    [Field1],
    [Field2]
FROM
    [Table1]")
                .Build(),
            new ViewBuilder().WithName("View2")
                .WithDefinition(@"SELECT TOP 50 PERCENT
    [Field2] AS [Alias2]
FROM
    [Table1],
    [Table2]
WHERE
    table1.Field1 = 'Value 1'
    AND table1.Field1 = 'Value 2'
GROUP BY
    [Field1],
    [Field2]
ORDER BY
    [table1.Field1] DESC,
    [table1.Field2] ASC")
                .Build()
        ];
    }

    private sealed class MultipleFilesCodeGenerationProvider : TestCodeGenerationProviderBase
    {
        public override DatabaseSchemaGeneratorSettings Settings => base.Settings.ToBuilder().WithGenerateMultipleFiles().Build();

        public override IEnumerable<IDatabaseObject> Model =>
        [
            new TableBuilder()
                .WithName("MyTable")
                .AddFields(new TableFieldBuilder().WithName("MyField").WithType(SqlFieldType.VarChar).WithStringLength(32))
                .Build(),
            new ViewBuilder().WithName("View1")
                .WithDefinition(@"SELECT
    [Field1],
    [Field2]
FROM
    [Table1]")
                .Build(),
            new StoredProcedureBuilder()
                .WithName("usp_Test")
                .AddParameters
                (
                    new StoredProcedureParameterBuilder().WithName("Param1").WithType(SqlFieldType.Int),
                    new StoredProcedureParameterBuilder().WithName("Param2").WithType(SqlFieldType.Int).WithDefaultValue("5")
                )
                .AddStatements
                (
                    new StringSqlStatementBuilder().WithStatement("--statement 1 goes here"),
                    new StringSqlStatementBuilder().WithStatement("--statement 2 goes here")
                )
            .Build()
        ];
    }
}
