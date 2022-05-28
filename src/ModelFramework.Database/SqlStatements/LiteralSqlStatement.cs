namespace ModelFramework.Database.SqlStatements;

public record LiteralSqlStatement : ISqlStatement
{
    public string Statement { get; }
    public IReadOnlyCollection<IMetadata> Metadata { get; }

    public LiteralSqlStatement(string statement, IEnumerable<IMetadata> metadata)
    {
        if (string.IsNullOrWhiteSpace(statement))
        {
            throw new ArgumentOutOfRangeException(nameof(statement), "Statement cannot be null or whitespace");
        }

        Statement = statement;
        Metadata = new ReadOnlyValueCollection<IMetadata>(metadata);
    }

    public ISqlStatementBuilder CreateBuilder()
        => new LiteralSqlStatementBuilder(this);

    public override string ToString() => Statement;
}
