using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class UniqueConstraintFieldBuilder
    {
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IUniqueConstraintField Build()
        {
            return new UniqueConstraintField(Name, Metadata.Select(x => x.Build()));
        }
        public UniqueConstraintFieldBuilder Clear()
        {
            Name = default;
            Metadata.Clear();
            return this;
        }
        public UniqueConstraintFieldBuilder Update(IUniqueConstraintField source)
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
        public UniqueConstraintFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public UniqueConstraintFieldBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public UniqueConstraintFieldBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public UniqueConstraintFieldBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public UniqueConstraintFieldBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public UniqueConstraintFieldBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public UniqueConstraintFieldBuilder(IUniqueConstraintField source = null)
        {
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Name = source.Name;
                if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            }
        }
        public UniqueConstraintFieldBuilder(string name, IEnumerable<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            Name = name;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
