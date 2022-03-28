namespace ModelFramework.Database.SqlStatements.Builders;

public partial class LiteralSqlStatementBuilder
{
    public LiteralSqlStatementBuilder(string statement)
    {
        Metadata = new List<Common.Builders.MetadataBuilder>();
        Statement = statement;
    }
}
