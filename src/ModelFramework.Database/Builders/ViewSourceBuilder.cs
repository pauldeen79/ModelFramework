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
                                  SourceObjectName,
                                  Alias,
                                  SourceSchemaName,
                                  Metadata.Select(x => x.Build()));
        }
        public ViewSourceBuilder Clear()
        {
            Alias = string.Empty;
            Name = string.Empty;
            SourceSchemaName = string.Empty;
            SourceObjectName = string.Empty;
            Metadata.Clear();
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
            Metadata.AddRange(metadata);
            return this;
        }
        public ViewSourceBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewSourceBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public ViewSourceBuilder()
        {
            Alias = string.Empty;
            Name = string.Empty;
            SourceSchemaName = string.Empty;
            SourceObjectName = string.Empty;
            Metadata = new List<MetadataBuilder>();
        }
        public ViewSourceBuilder(IViewSource source)
        {
            Metadata = new List<MetadataBuilder>();

            Alias = source.Alias;
            Name = source.Name;
            SourceSchemaName = source.SourceSchemaName;
            SourceObjectName = source.SourceObjectName;
            Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
