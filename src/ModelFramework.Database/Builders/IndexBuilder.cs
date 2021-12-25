using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class IndexBuilder
    {
        public List<IndexFieldBuilder> Fields { get; set; }
        public bool Unique { get; set; }
        public string Name { get; set; }
        public string FileGroupName { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IIndex Build()
        {
            return new Index(Name,
                             Unique,
                             FileGroupName,
                             Fields.Select(x => x.Build()),
                             Metadata.Select(x => x.Build()));
        }
        public IndexBuilder Clear()
        {
            Fields.Clear();
            Unique = default;
            Name = string.Empty;
            FileGroupName = string.Empty;
            Metadata.Clear();
            return this;
        }
        public IndexBuilder ClearFields()
        {
            Fields.Clear();
            return this;
        }
        public IndexBuilder AddFields(IEnumerable<IndexFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }
        public IndexBuilder AddFields(params IndexFieldBuilder[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }
        public IndexBuilder AddFields(IEnumerable<IndexField> fields)
        {
            return AddFields(fields.ToArray());
        }
        public IndexBuilder AddFields(params IndexField[] fields)
        {
            Fields.AddRange(fields.Select(itemToAdd => new IndexFieldBuilder(itemToAdd)));
            return this;
        }
        public IndexBuilder WithUnique(bool unique = true)
        {
            Unique = unique;
            return this;
        }
        public IndexBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public IndexBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }
        public IndexBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public IndexBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public IndexBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public IndexBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public IndexBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public IndexBuilder()
        {
            FileGroupName = string.Empty;
            Name = string.Empty;
            Fields = new List<IndexFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
        }
        public IndexBuilder(IIndex source)
        {
            Fields = new List<IndexFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
            Fields.AddRange(source.Fields.Select(x => new IndexFieldBuilder(x)));
            Unique = source.Unique;
            Name = source.Name;
            FileGroupName = source.FileGroupName;
            Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
