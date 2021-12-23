using System.Diagnostics.CodeAnalysis;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.Database.Builders;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using ModelFramework.Database.SqlStatements;
using ModelFramework.Generators.Database;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Tests.Database
{
    [ExcludeFromCodeCoverage]
    public class SqlServerDatabaseSchemaGeneratorTests
    {
        [Fact]
        public void CanGenerateSchemaForTables()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields(new TableFieldBuilder().WithName("Field").WithType(SqlTableFieldTypes.Int)),
                    new TableBuilder().WithName("Table2").AddFields(new TableFieldBuilder().WithName("Field").WithType(SqlTableFieldTypes.Int)),
                    new TableBuilder().WithName("Table3").AddFields(new TableFieldBuilder().WithName("Field").WithType(SqlTableFieldTypes.Int))
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { CreateCodeGenerationHeader = true });

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"/****** Object:  Table [dbo].[Table1] ******/
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
        public void CanGenerateSchemaForTableWithFields()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int),
                        new TableFieldBuilder().WithName("Field2").WithType(SqlTableFieldTypes.VarChar).WithStringLength(32),
                        new TableFieldBuilder().WithName("Field3").WithType(SqlTableFieldTypes.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
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
");
        }

        [Fact]
        public void CanGenerateSchemaForTableWithCheckConstraintOnFieldLevel()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo")
                .AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int).AddCheckConstraints(new CheckConstraintBuilder().WithName("CHK1").WithExpression("[Field1] BETWEEN 1 AND 10")),
                        new TableFieldBuilder().WithName("Field2").WithType(SqlTableFieldTypes.VarChar).WithStringLength(32),
                        new TableFieldBuilder().WithName("Field3").WithType(SqlTableFieldTypes.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
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
        public void CanGenerateSchemaForTableWithCheckContraintOnTableLevel()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int),
                        new TableFieldBuilder().WithName("Field2").WithType(SqlTableFieldTypes.Int)
                    ).AddCheckConstraints
                    (
                        new CheckConstraintBuilder().WithName("MyCheckContraint1").WithExpression("Field1 > 10"),
                        new CheckConstraintBuilder().WithName("MyCheckContraint2").WithExpression("Field2 > 20")
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL,
	[Field2] INT NULL,
    CONSTRAINT [MyCheckContraint1]
    CHECK (Field1 > 10),
    CONSTRAINT [MyCheckContraint2]
    CHECK (Field2 > 20)
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
");
        }

        [Fact]
        public void CanGenerateSchemaForTableWithIndexes()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int)
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
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
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
        public void CanGenerateSchemaForTableWithPrimaryKeyConstraint()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int),
                        new TableFieldBuilder().WithName("Field2").WithType(SqlTableFieldTypes.VarChar).WithStringLength(32),
                        new TableFieldBuilder().WithName("Field3").WithType(SqlTableFieldTypes.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
                    ).AddPrimaryKeyConstraints
                    (
                        new PrimaryKeyConstraintBuilder().WithName("PK").AddFields
                        (
                            new PrimaryKeyConstraintFieldBuilder().WithName("Field1")
                        )
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
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
        public void CanGenerateSchemaForTableWithUniqueConstraint()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int),
                        new TableFieldBuilder().WithName("Field2").WithType(SqlTableFieldTypes.VarChar).WithStringLength(32),
                        new TableFieldBuilder().WithName("Field3").WithType(SqlTableFieldTypes.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
                    ).AddUniqueConstraints
                    (
                        new UniqueConstraintBuilder().WithName("UC").AddFields
                        (
                            new UniqueConstraintFieldBuilder().WithName("Field1")
                        )
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { CreateCodeGenerationHeader = true});

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"/****** Object:  Table [dbo].[Table1] ******/
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
        public void CanGenerateSchemaForTableWithDefaultValueConstraint()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int),
                        new TableFieldBuilder().WithName("Field2").WithType(SqlTableFieldTypes.VarChar).WithStringLength(32),
                        new TableFieldBuilder().WithName("Field3").WithType(SqlTableFieldTypes.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
                    ).AddDefaultValueConstraints
                    (
                        new DefaultValueConstraintBuilder().WithFieldName("Field1").WithDefaultValue("2").WithName("DVC")
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
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
        public void CanGenerateSchemaForTableWithStoredProcedureContainingStatements()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddStoredProcedures
                (
                    new StoredProcedureBuilder().WithName("usp_Test").AddParameters
                    (
                        new StoredProcedureParameterBuilder().WithName("Param1").WithType("int"),
                        new StoredProcedureParameterBuilder().WithName("Param2").WithType("int").WithDefaultValue("5")
                    ).AddStatements
                    (
                        new LiteralSqlStatement("--statement 1 goes here"),
                        new LiteralSqlStatement("--statement 2 goes here")
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Test]
	@Param1 int,
	@Param2 int = 5
AS
BEGIN
    --statement 1 goes here
    --statement 2 goes here
END
GO
");
        }

        [Fact]
        public void CanGenerateSchemaForTableWithForeignKeyConstraint()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int),
                        new TableFieldBuilder().WithName("Field2").WithType(SqlTableFieldTypes.VarChar).WithStringLength(32),
                        new TableFieldBuilder().WithName("Field3").WithType(SqlTableFieldTypes.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
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
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
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
        public void CanGenerateSchemaForTableWithCascadeForeignKeyConstraint()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int),
                        new TableFieldBuilder().WithName("Field2").WithType(SqlTableFieldTypes.VarChar).WithStringLength(32),
                        new TableFieldBuilder().WithName("Field3").WithType(SqlTableFieldTypes.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
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
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
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
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Table1] CHECK CONSTRAINT [FK]
GO
");
        }

        [Fact]
        public void CanGenerateSchemaForTableWithIdentityField()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddTables
                (
                    new TableBuilder().WithName("Table1").AddFields
                    (
                        new TableFieldBuilder().WithName("Field1").WithType(SqlTableFieldTypes.Int).WithIsIdentity(),
                        new TableFieldBuilder().WithName("Field2").WithType(SqlTableFieldTypes.VarChar).WithStringLength(32),
                        new TableFieldBuilder().WithName("Field3").WithType(SqlTableFieldTypes.Numeric).WithIsRequired().WithNumericPrecision(8).WithNumericScale(2)
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
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
        public void CanGenerateSchemaForViews()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new SchemaBuilder().WithName("dbo").AddViews
                (
                    new ViewBuilder().WithName("View1").AddSources
                    (
                        new ViewSourceBuilder().WithName("Table1")
                    ).AddSelectFields
                    (
                        new ViewFieldBuilder().WithName("Field1"),
                        new ViewFieldBuilder().WithName("Field2")
                    ),
                    new ViewBuilder().WithName("View2").AddSources
                    (
                        new ViewSourceBuilder().WithName("Table1"),
                        new ViewSourceBuilder().WithName("Table2")
                    ).AddSelectFields
                    (
                        new ViewFieldBuilder().WithName("Field2").WithAlias("Alias2")
                    ).AddConditions
                    (
                        new ViewConditionBuilder().WithExpression("table1.Field1 = 'Value 1'").WithCombination("AND"),
                        new ViewConditionBuilder().WithExpression("table1.Field1 = 'Value 2'").WithCombination("AND")
                    ).AddOrderByFields
                    (
                        new ViewOrderByFieldBuilder().WithName("table1.Field1").WithDescending(),
                        new ViewOrderByFieldBuilder().WithName("table1.Field2")
                    ).AddGroupByFields
                    (
                        new ViewField("Field1"),
                        new ViewField("Field2")
                    )
                ).Build()
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"SET ANSI_NULLS ON
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
SELECT
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
    }
}
