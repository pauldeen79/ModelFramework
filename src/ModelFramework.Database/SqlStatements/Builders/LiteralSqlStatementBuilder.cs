using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.SqlStatements.Builders
{
    public class LiteralSqlStatementBuilder : ISqlStatementBuilder
    {
        public string Statement { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }

        public LiteralSqlStatementBuilder WithStatement(string statement)
        {
            Statement = statement;
            return this;
        }
        public LiteralSqlStatementBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public LiteralSqlStatementBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public ISqlStatement Build()
        {
            return new LiteralSqlStatement(Statement, Metadata.Select(x => x.Build()));
        }
        public LiteralSqlStatementBuilder()
        {
            Statement = string.Empty;
            Metadata = new List<MetadataBuilder>();
        }
        public LiteralSqlStatementBuilder(LiteralSqlStatement source)
        {
            Statement = source.Statement;
            Metadata = new List<MetadataBuilder>(source.Metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
