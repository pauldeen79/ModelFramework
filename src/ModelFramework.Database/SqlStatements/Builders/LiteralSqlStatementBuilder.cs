namespace ModelFramework.Database.SqlStatements.Builders;

public partial class LiteralSqlStatementBuilder
{
    public LiteralSqlStatementBuilder(string statement)
    {
        _statementDelegate = new Lazy<string>();
        Metadata = new List<Common.Builders.MetadataBuilder>();
        Statement = statement;
    }
}
