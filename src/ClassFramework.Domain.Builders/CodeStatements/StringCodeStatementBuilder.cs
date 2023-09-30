namespace ClassFramework.Domain.Builders.CodeStatements;

public partial class StringCodeStatementBuilder
{
    public StringCodeStatementBuilder(string statement)
    {
        Statement = statement.IsNotNull(nameof(statement));
    }
}
