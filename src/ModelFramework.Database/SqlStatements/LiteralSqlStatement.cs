using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.SqlStatements.Builders;

namespace ModelFramework.Database.SqlStatements
{
    public record LiteralSqlStatement : ISqlStatement
    {
        public string Statement { get; }
        public ValueCollection<IMetadata> Metadata { get; }

        public LiteralSqlStatement(string statement, IEnumerable<IMetadata>metadata)
        {
            if (string.IsNullOrWhiteSpace(statement))
            {
                throw new ArgumentOutOfRangeException(nameof(statement), "Statement cannot be null or whitespace");
            }

            Statement = statement;
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public ISqlStatementBuilder CreateBuilder()
            => new LiteralSqlStatementBuilder(this);

        public override string ToString() => Statement;
    }
}
