using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
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
                new Schema("dbo", new[] { new Table("Table1"), new Table("Table2"), new Table("Table3") })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { CreateCodeGenerationHeader = true });

            // Assert
            actual.Should().Be(@"/****** Object:  Table [dbo].[Table1] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
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
                new Schema("dbo", new[] { new Table("Table1", fields: new[] { new TableField("Field1", SqlTableFieldTypes.Int), new TableField("Field2", SqlTableFieldTypes.VarChar, stringLength: 32), new TableField("Field3", SqlTableFieldTypes.Numeric, true, numericPrecision: 8, numericScale: 2) }) })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
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
        public void CanGenerateSchemaForTableWithCheckConstraint()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new Schema("dbo", new[] { new Table("Table1", fields: new[] { new TableField("Field1", SqlTableFieldTypes.Int, checkConstraint: new TableFieldCheckConstraint("CHK1", "[Field1] BETWEEN 1 AND 10")), new TableField("Field2", SqlTableFieldTypes.VarChar, stringLength: 32), new TableField("Field3", SqlTableFieldTypes.Numeric, true, numericPrecision: 8, numericScale: 2) }) })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Table1](
	[Field1] INT NULL
    CONSTRAINT CHK1
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
        public void CanGenerateSchemaForTableWithIndexes()
        {
            // Arrange
            var sut = new SqlServerDatabaseSchemaGenerator();
            var model = new[]
            {
                new Schema("dbo", new[] { new Table("Table1", fields: new[] { new TableField("Field1", SqlTableFieldTypes.Int) }, indexes: new[] { new Index("IX_Index1", true, new[] { new IndexField("Field1"), new IndexField("Field2", true) }), new Index("IX_Index2", false, new[] { new IndexField("Field1"), new IndexField("Field2", true) }) }) })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
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
                new Schema("dbo", new[] { new Table("Table1", fields: new[] { new TableField("Field1", SqlTableFieldTypes.Int), new TableField("Field2", SqlTableFieldTypes.VarChar, stringLength: 32), new TableField("Field3", SqlTableFieldTypes.Numeric, true, numericPrecision: 8, numericScale: 2) }, primaryKeyConstraints: new[] { new PrimaryKeyConstraint("PK", fields: new[] { new PrimaryKeyConstraintField("Field1") }) }) }) 
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
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
                new Schema("dbo", new[] { new Table("Table1", fields: new[] { new TableField("Field1", SqlTableFieldTypes.Int), new TableField("Field2", SqlTableFieldTypes.VarChar, stringLength: 32), new TableField("Field3", SqlTableFieldTypes.Numeric, true, numericPrecision: 8, numericScale: 2) }, uniqueConstraints: new[] { new UniqueConstraint("UC", fields: new[] { new UniqueConstraintField("Field1") }) }) })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model, additionalParameters: new { CreateCodeGenerationHeader = true});

            // Assert
            actual.Should().Be(@"/****** Object:  Table [dbo].[Table1] ******/
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
                new Schema("dbo", new[] { new Table("Table1", fields: new[] { new TableField("Field1", SqlTableFieldTypes.Int), new TableField("Field2", SqlTableFieldTypes.VarChar, stringLength: 32), new TableField("Field3", SqlTableFieldTypes.Numeric, true, numericPrecision: 8, numericScale: 2) }, defaultValueConstraints: new[] { new DefaultValueConstraint("Field1", "2", "DVC") }) })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
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
                new Schema("dbo", null, new[] { new StoredProcedure("usp_Test", new[] { new StoredProcedureParameter("Param1", "int", null), new StoredProcedureParameter("Param2", "int", "5") }, new[] { new LiteralSqlStatement("--statement 1 goes here"), new LiteralSqlStatement("--statement 2 goes here") }) })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
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
                new Schema("dbo", new[] { new Table("Table1", fields: new[] { new TableField("Field1", SqlTableFieldTypes.Int), new TableField("Field2", SqlTableFieldTypes.VarChar, stringLength: 32), new TableField("Field3", SqlTableFieldTypes.Numeric, true, numericPrecision: 8, numericScale: 2) }, foreignKeyConstraints: new[] { new ForeignKeyConstraint("FK", "ForeignTable", new[] { new ForeignKeyConstraintField("LocalField1"), new ForeignKeyConstraintField("LocalField2") }, new[] { new ForeignKeyConstraintField("RemoteField1"), new ForeignKeyConstraintField("RemoteField2") }) }) })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
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
                new Schema("dbo", new[] { new Table("Table1", fields: new[] { new TableField("Field1", SqlTableFieldTypes.Int), new TableField("Field2", SqlTableFieldTypes.VarChar, stringLength: 32), new TableField("Field3", SqlTableFieldTypes.Numeric, true, numericPrecision: 8, numericScale: 2) }, foreignKeyConstraints: new[] { new ForeignKeyConstraint("FK", "ForeignTable", new[] { new ForeignKeyConstraintField("LocalField1"), new ForeignKeyConstraintField("LocalField2") }, new[] { new ForeignKeyConstraintField("RemoteField1"), new ForeignKeyConstraintField("RemoteField2") }, CascadeAction.Cascade, CascadeAction.Cascade) }) })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
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
                new Schema("dbo", new[] { new Table("Table1", fields: new[] { new TableField("Field1", SqlTableFieldTypes.Int, isIdentity: true), new TableField("Field2", SqlTableFieldTypes.VarChar, stringLength: 32), new TableField("Field3", SqlTableFieldTypes.Numeric, true, numericPrecision: 8, numericScale: 2) }) })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
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
                new Schema("dbo", views: new[]
                {
                    new View("View1", sources: new[] { new ViewSource("Table1") }, selectFields: new[] { new ViewField("Field1"), new ViewField("Field2") }),
                    new View("View2", sources: new[] { new ViewSource("Table1"), new ViewSource("Table2") }, selectFields: new[] { /*new ViewField("Field1", sourceSchemaName: "table1"), */new ViewField("Field2", alias: "Alias2") }, conditions: new[] { new ViewCondition("AND", "table1.Field1 = 'Value 1'"), new ViewCondition("AND", "table1.Field1 = 'Value 2'") }, orderByFields: new[] { new ViewOrderByField("table1.Field1", descending: true), new ViewOrderByField("table1.Field2") }, groupByFields: new[] { new ViewField("Field1"), new ViewField("Field2") } )
                })
            };

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"SET ANSI_NULLS ON
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
