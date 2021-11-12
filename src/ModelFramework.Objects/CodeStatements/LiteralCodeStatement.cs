using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.CodeStatements.Builders;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.CodeStatements
{
    public class LiteralCodeStatement : ICodeStatement
    {
        public ValueCollection<IMetadata> Metadata { get; }
        public string Statement { get; }

        public LiteralCodeStatement(string statement, IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(statement)) throw new ArgumentOutOfRangeException(nameof(statement), "Statement cannot be null or whitespace");

            Statement = statement;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public ICodeStatementBuilder CreateBuilder()
            => new LiteralCodeStatementBuilder(this);

        public override string ToString() => Statement;
    }
}
