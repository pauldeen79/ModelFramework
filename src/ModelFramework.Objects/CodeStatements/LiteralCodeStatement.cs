﻿namespace ModelFramework.Objects.CodeStatements;

public record LiteralCodeStatement : ICodeStatement
{
    public IReadOnlyCollection<IMetadata> Metadata { get; }
    public string Statement { get; }

    public LiteralCodeStatement(string statement, IEnumerable<IMetadata> metadata)
    {
        if (string.IsNullOrWhiteSpace(statement))
        {
            throw new ArgumentOutOfRangeException(nameof(statement), "Statement cannot be null or whitespace");
        }

        Statement = statement;
        Metadata = new ReadOnlyValueCollection<IMetadata>(metadata);
    }

    public ICodeStatementBuilder CreateBuilder()
        => new LiteralCodeStatementBuilder(this);

    public override string ToString() => Statement;

    public ICodeStatementModel CreateModel() => new LiteralCodeStatementModel(this);
}
