namespace ModelFramework.Objects.CodeStatements.Builders;

public partial class LiteralCodeStatementBuilder
{
    public LiteralCodeStatementBuilder(string statement)
    {
        _statementDelegate = new Lazy<string>();
        Metadata = new List<MetadataBuilder>();
        Statement = statement;
    }
}
