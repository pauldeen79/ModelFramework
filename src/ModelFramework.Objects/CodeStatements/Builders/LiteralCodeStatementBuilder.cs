namespace ModelFramework.Objects.CodeStatements.Builders;

public partial class LiteralCodeStatementBuilder
{
    public LiteralCodeStatementBuilder(string statement)
    {
        Metadata = new List<MetadataBuilder>();
        Statement = statement;
    }
}
