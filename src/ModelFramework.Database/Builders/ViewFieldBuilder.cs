using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class ViewFieldBuilder
    {
        public string SourceSchemaName { get; set; }
        public string SourceObjectName { get; set; }
        public string Expression { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IViewField Build()
        {
            return new ViewField(Name,
                                 SourceSchemaName,
                                 SourceObjectName,
                                 Expression,
                                 Alias,
                                 Metadata.Select(x => x.Build()));
        }
        public ViewFieldBuilder Clear()
        {
            SourceSchemaName = default;
            SourceObjectName = default;
            Expression = default;
            Alias = default;
            Name = default;
            Metadata.Clear();
            return this;
        }
        public ViewFieldBuilder Update(ViewField source)
        {
            Metadata = new List<MetadataBuilder>();

            SourceSchemaName = source.SourceSchemaName;
            SourceObjectName = source.SourceObjectName;
            Expression = source.Expression;
            Alias = source.Alias;
            Name = source.Name;
            if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));

            return this;
        }
        public ViewFieldBuilder WithSourceSchemaName(string sourceSchemaName)
        {
            SourceSchemaName = sourceSchemaName;
            return this;
        }
        public ViewFieldBuilder WithSourceObjectName(string sourceObjectName)
        {
            SourceObjectName = sourceObjectName;
            return this;
        }
        public ViewFieldBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }
        public ViewFieldBuilder WithAlias(string alias)
        {
            Alias = alias;
            return this;
        }
        public ViewFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ViewFieldBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ViewFieldBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewFieldBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public ViewFieldBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewFieldBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ViewFieldBuilder()
        {
            Metadata = new List<MetadataBuilder>();
        }
        public ViewFieldBuilder(IViewField source)
        {
            Metadata = new List<MetadataBuilder>();

            SourceSchemaName = source.SourceSchemaName;
            SourceObjectName = source.SourceObjectName;
            Expression = source.Expression;
            Alias = source.Alias;
            Name = source.Name;
            if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
        }
    }
}
