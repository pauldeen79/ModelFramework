using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using System.Collections.Generic;
using System.Linq;

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
            return new ViewField(Name, SourceSchemaName, SourceObjectName, Expression, Alias, Metadata.Select(x => x.Build()));
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
        public ViewFieldBuilder(IViewField source = null)
        {
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                SourceSchemaName = source.SourceSchemaName;
                SourceObjectName = source.SourceObjectName;
                Expression = source.Expression;
                Alias = source.Alias;
                Name = source.Name;
                foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            }
        }
        public ViewFieldBuilder(string name,
                                string sourceSchemaName = null,
                                string sourceObjectName = null,
                                string expression = null,
                                string alias = null,
                                IEnumerable<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            SourceSchemaName = sourceSchemaName;
            SourceObjectName = sourceObjectName;
            Expression = expression;
            Alias = alias;
            Name = name;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
