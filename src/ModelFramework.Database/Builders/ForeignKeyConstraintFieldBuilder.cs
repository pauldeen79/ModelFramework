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
            Name = default;
            Metadata.Clear();
            return this;
        }
        public ForeignKeyConstraintFieldBuilder Update(ForeignKeyConstraintField source)
        {
            Name = default;
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Name = source.Name;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ForeignKeyConstraintFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ForeignKeyConstraintFieldBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ForeignKeyConstraintFieldBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                foreach (var itemToAdd in metadata)
                {
                    Metadata.Add(itemToAdd);
                }
            }
            return this;
        }
        public ForeignKeyConstraintFieldBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ForeignKeyConstraintFieldBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ForeignKeyConstraintFieldBuilder(IForeignKeyConstraintField source = null)
        {
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Name = source.Name;
                if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            }
        }
        public ForeignKeyConstraintFieldBuilder(string name, IEnumerable<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            Name = name;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
