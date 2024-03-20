namespace DatabaseFramework.Domain.Tests;

public class StoredProcedureTests
{
    [Fact]
    public void Can_Construct_With_Parameters_And_Code_Body()
    {
        // Arrange
        var builder = new StoredProcedureBuilder()
            .WithName("usp_MyProc")
            .AddStatements(new StringSqlStatementBuilder().WithStatement("-- hello world!"))
            .AddParameters(new StoredProcedureParameterBuilder()
                .WithType(SqlFieldType.VarChar)
                .WithStringLength(32)
                .WithName("MyParameter")
                .WithDefaultValue("my default value"));

        // Act
        var storedProcedure = builder.Build();

        // Assert
        storedProcedure.Name.Should().Be("usp_MyProc");
    }
}
