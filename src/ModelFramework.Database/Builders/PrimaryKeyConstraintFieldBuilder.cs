using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class PrimaryKeyConstraintFieldBuilder
    {
        public bool IsDescending { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IPrimaryKeyConstraintField Build()
        {
            return new PrimaryKeyConstraintField(Name,
                                                 IsDescending,
                                                 Metadata.Select(x => x.Build()));
        }
        public PrimaryKeyConstraintFieldBuilder Clear()
        {
            IsDescending = default;
            Name = string.Empty;
            Metadata.Clear();
            return this;
        }
        public PrimaryKeyConstraintFieldBuilder WithIsDescending(bool isDescending = true)
        {
            IsDescending = isDescending;
            return this;
        }
        public PrimaryKeyConstraintFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public PrimaryKeyConstraintFieldBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public PrimaryKeyConstraintFieldBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public PrimaryKeyConstraintFieldBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public PrimaryKeyConstraintFieldBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public PrimaryKeyConstraintFieldBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public PrimaryKeyConstraintFieldBuilder()
        {
            Name = string.Empty;
            Metadata = new List<MetadataBuilder>();
        }
        public PrimaryKeyConstraintFieldBuilder(IPrimaryKeyConstraintField source)
        {
            Metadata = new List<MetadataBuilder>();

            IsDescending = source.IsDescending;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
