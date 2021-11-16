using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class ViewSourceBuilder
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public string SourceSchemaName { get; set; }
        public string SourceObjectName { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IViewSource Build()
        {
            return new ViewSource(Name,
                                  Alias,
                                  SourceSchemaName,
                                  SourceObjectName,
                                  Metadata.Select(x => x.Build()));
        }
        public ViewSourceBuilder Clear()
        {
            Alias = default;
            Name = default;
            SourceSchemaName = default;
            SourceObjectName = default;
            Metadata.Clear();
            return this;
        }
        public ViewSourceBuilder Update(IViewSource source)
        {

            Metadata = new List<MetadataBuilder>();

            Alias = source.Alias;
            Name = source.Name;
            SourceSchemaName = source.SourceSchemaName;
            SourceObjectName = source.SourceObjectName;
            if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));

            return this;
        }
        public ViewSourceBuilder WithAlias(string alias)
        {
            Alias = alias;
            return this;
        }
        public ViewSourceBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ViewSourceBuilder WithSourceSchemaName(string sourceSchemaName)
        {
            SourceSchemaName = sourceSchemaName;
            return this;
        }
        public ViewSourceBuilder WithSourceObjectName(string sourceObjectName)
        {
            SourceObjectName = sourceObjectName;
            return this;
        }
        public ViewSourceBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ViewSourceBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewSourceBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public ViewSourceBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewSourceBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ViewSourceBuilder()
        {
            Metadata = new List<MetadataBuilder>();
        }
        public ViewSourceBuilder(IViewSource source)
        {
            Metadata = new List<MetadataBuilder>();

            Alias = source.Alias;
            Name = source.Name;
            SourceSchemaName = source.SourceSchemaName;
            SourceObjectName = source.SourceObjectName;
            if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
        }
    }
}
