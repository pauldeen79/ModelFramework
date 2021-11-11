using ModelFramework.Common.Contracts;
using ModelFramework.Objects.CodeStatements.Builders;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.CodeStatements
{
    public class LiteralCodeStatement : ICodeStatement
    {
        public IReadOnlyCollection<IMetadata> Metadata { get; }
        public string Statement { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Class" /> class.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Statement cannot be null or whitespace</exception>
        public LiteralCodeStatement(string statement
            , IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(statement)) throw new ArgumentOutOfRangeException(nameof(statement), "Statement cannot be null or whitespace");

            Statement = statement;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public ICodeStatementBuilder CreateBuilder()
            => new LiteralCodeStatementBuilder(this);

        public override string ToString() => Statement;
    }
}
