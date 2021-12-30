using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.CodeStatements.Builders
{
    public class LiteralCodeStatementBuilder : ICodeStatementBuilder
    {
        public string Statement { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }

        public LiteralCodeStatementBuilder WithStatement(string statement)
        {
            Statement = statement;
            return this;
        }
        public LiteralCodeStatementBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public LiteralCodeStatementBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public ICodeStatement Build()
        {
            return new LiteralCodeStatement(Statement, Metadata.Select(x => x.Build()));
        }
        public LiteralCodeStatementBuilder Clear()
        {
            Statement = string.Empty;
            Metadata.Clear();
            return this;
        }
        public LiteralCodeStatementBuilder()
        {
            Statement = string.Empty;
            Metadata = new List<MetadataBuilder>();
        }
        public LiteralCodeStatementBuilder(LiteralCodeStatement source)
        {
            Statement = source.Statement;
            Metadata = new List<MetadataBuilder>(source.Metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
