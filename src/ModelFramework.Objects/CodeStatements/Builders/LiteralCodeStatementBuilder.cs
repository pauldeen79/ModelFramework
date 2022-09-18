namespace ModelFramework.Objects.CodeStatements.Builders;

public partial class LiteralCodeStatementBuilder
{
    public LiteralCodeStatementBuilder(string statement)
    {
        _statementDelegate = new Lazy<StringBuilder>();
        Metadata = new List<MetadataBuilder>();
        Statement.Append(statement);
    }
}
