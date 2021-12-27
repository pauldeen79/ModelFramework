using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class ForeignKeyConstraintFieldBuilder
    {
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IForeignKeyConstraintField Build()
        {
            return new ForeignKeyConstraintField(Name, Metadata.Select(x => x.Build()));
        }
        public ForeignKeyConstraintFieldBuilder Clear()
        {
            Name = string.Empty;
            Metadata.Clear();
            return this;
        }
        public ForeignKeyConstraintFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ForeignKeyConstraintFieldBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ForeignKeyConstraintFieldBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ForeignKeyConstraintFieldBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public ForeignKeyConstraintFieldBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ForeignKeyConstraintFieldBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public ForeignKeyConstraintFieldBuilder()
        {
            Name = string.Empty;
            Metadata = new List<MetadataBuilder>();
        }
        public ForeignKeyConstraintFieldBuilder(IForeignKeyConstraintField source)
        {
            Metadata = new List<MetadataBuilder>();

            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
