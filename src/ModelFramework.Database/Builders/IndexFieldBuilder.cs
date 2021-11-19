using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class IndexFieldBuilder
    {
        public bool IsDescending { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IIndexField Build()
        {
            return new IndexField(Name,
                                  IsDescending,
                                  Metadata.Select(x => x.Build()));
        }
        public IndexFieldBuilder Clear()
        {
            IsDescending = default;
            Name = default;
            Metadata.Clear();
            return this;
        }
        public IndexFieldBuilder WithIsDescending(bool isDescending)
        {
            IsDescending = isDescending;
            return this;
        }
        public IndexFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public IndexFieldBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public IndexFieldBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public IndexFieldBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public IndexFieldBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public IndexFieldBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public IndexFieldBuilder()
        {
            Metadata = new List<MetadataBuilder>();
        }
        public IndexFieldBuilder(IIndexField source)
        {
            Metadata = new List<MetadataBuilder>();

            IsDescending = source.IsDescending;
            Name = source.Name;
            if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
        }
    }
}
