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
            return new Index(Name, Unique, Fields.Select(x => x.Build()), FileGroupName, Metadata.Select(x => x.Build()));
        }
        public IndexBuilder Clear()
        {
            Fields.Clear();
            Unique = default;
            Name = default;
            FileGroupName = default;
            Metadata.Clear();
            return this;
        }
        public IndexBuilder Update(Index source)
        {
            Fields = new List<IndexFieldBuilder>();
            Unique = default;
            Name = default;
            FileGroupName = default;
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                if (source.Fields != null) Fields.AddRange(source.Fields.Select(x => new IndexFieldBuilder(x)));
                Unique = source.Unique;
                Name = source.Name;
                FileGroupName = source.FileGroupName;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
            }
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
            if (fields != null)
            {
                foreach (var itemToAdd in fields)
                {
                    Fields.Add(itemToAdd);
                }
            }
            return this;
        }
        public IndexBuilder AddFields(IEnumerable<IndexField> fields)
        {
            return AddFields(fields.ToArray());
        }
        public IndexBuilder AddFields(params IndexField[] fields)
        {
            if (fields != null)
            {
                foreach (var itemToAdd in fields)
                {
                    Fields.Add(new IndexFieldBuilder(itemToAdd));
                }
            }
            return this;
        }
        public IndexBuilder WithUnique(bool unique)
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
            if (metadata != null)
            {
                foreach (var itemToAdd in metadata)
                {
                    Metadata.Add(itemToAdd);
                }
            }
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
        public IndexBuilder(IIndex source = null)
        {
            Fields = new List<IndexFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                if (source.Fields != null) foreach (var x in source.Fields) Fields.Add(new IndexFieldBuilder(x));
                Unique = source.Unique;
                Name = source.Name;
                FileGroupName = source.FileGroupName;
                if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            }
        }
        public IndexBuilder(string name,
                            bool unique,
                            IEnumerable<IIndexField> fields,
                            string fileGroupName = null,
                            IEnumerable<IMetadata> metadata = null)
        {
            Fields = new List<IndexFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
            Name = name;
            Unique = unique;
            if (fields != null) Fields.AddRange(fields.Select(x => new IndexFieldBuilder(x)));
            FileGroupName = fileGroupName;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
