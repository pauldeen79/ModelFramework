using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class ViewOrderByFieldBuilder
    {
        public bool Descending { get; set; }
        public string SourceSchemaName { get; set; }
        public string SourceObjectName { get; set; }
        public string Expression { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IViewOrderByField Build()
        {
            return new ViewOrderByField(Name,
                                        SourceSchemaName,
                                        SourceObjectName,
                                        Expression,
                                        Alias,
                                        Descending,
                                        Metadata.Select(x => x.Build()));
        }
        public ViewOrderByFieldBuilder Clear()
        {
            Descending = default;
            SourceSchemaName = default;
            SourceObjectName = default;
            Expression = default;
            Alias = default;
            Name = default;
            Metadata.Clear();
            return this;
        }
        public ViewOrderByFieldBuilder Update(IViewOrderByField source)
        {
            Metadata = new List<MetadataBuilder>();

            Descending = source.Descending;
            SourceSchemaName = source.SourceSchemaName;
            SourceObjectName = source.SourceObjectName;
            Expression = source.Expression;
            Alias = source.Alias;
            Name = source.Name;
            if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));

            return this;
        }
        public ViewOrderByFieldBuilder WithDescending(bool descending)
        {
            Descending = descending;
            return this;
        }
        public ViewOrderByFieldBuilder WithSourceSchemaName(string sourceSchemaName)
        {
            SourceSchemaName = sourceSchemaName;
            return this;
        }
        public ViewOrderByFieldBuilder WithSourceObjectName(string sourceObjectName)
        {
            SourceObjectName = sourceObjectName;
            return this;
        }
        public ViewOrderByFieldBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }
        public ViewOrderByFieldBuilder WithAlias(string alias)
        {
            Alias = alias;
            return this;
        }
        public ViewOrderByFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ViewOrderByFieldBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ViewOrderByFieldBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewOrderByFieldBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public ViewOrderByFieldBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewOrderByFieldBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ViewOrderByFieldBuilder()
        {
            Metadata = new List<MetadataBuilder>();
        }
        public ViewOrderByFieldBuilder(IViewOrderByField source)
        {
            Metadata = new List<MetadataBuilder>();

            Descending = source.Descending;
            SourceSchemaName = source.SourceSchemaName;
            SourceObjectName = source.SourceObjectName;
            Expression = source.Expression;
            Alias = source.Alias;
            Name = source.Name;
            if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
        }
    }
}
