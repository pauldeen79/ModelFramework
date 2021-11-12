using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
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
        public LiteralCodeStatementBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public LiteralCodeStatementBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public LiteralCodeStatementBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public LiteralCodeStatementBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public LiteralCodeStatementBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ICodeStatement Build()
        {
            return new LiteralCodeStatement(Statement, Metadata.Select(x => x.Build()));
        }
        public LiteralCodeStatementBuilder Clear()
        {
            Statement = default;
            Metadata.Clear();
            return this;
        }
        public LiteralCodeStatementBuilder Update(LiteralCodeStatement source)
        {
            Metadata = new List<MetadataBuilder>();
            Statement = default;
            if (source != null)
            {
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                Statement = source.Statement;
            }
            return this;
        }
        public LiteralCodeStatementBuilder(LiteralCodeStatement source = null)
        {
            if (source != null)
            {
                Statement = source.Statement;
                Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
            }
            else
            {
                Metadata = new List<MetadataBuilder>();
            }
        }
        public LiteralCodeStatementBuilder(string statement, IEnumerable<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            Statement = statement;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
