using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class TableFieldCheckConstraintBuilder
    {
        public string Expression { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public ITableFieldCheckConstraint Build()
        {
            return new TableFieldCheckConstraint(Expression, Name, Metadata.Select(x => x.Build()));
        }
        public TableFieldCheckConstraintBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }
        public TableFieldCheckConstraintBuilder Clear()
        {
            Expression = default;
            Name = default;
            Metadata.Clear();
            return this;
        }
        public TableFieldCheckConstraintBuilder Update(ITableFieldCheckConstraint source)
        {
            Expression = default;
            Name = default;
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Expression = source.Expression;
                Name = source.Name;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }

        public TableFieldCheckConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public TableFieldCheckConstraintBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public TableFieldCheckConstraintBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public TableFieldCheckConstraintBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public TableFieldCheckConstraintBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public TableFieldCheckConstraintBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public TableFieldCheckConstraintBuilder(ITableFieldCheckConstraint source = null)
        {
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Expression = source.Expression;
                Name = source.Name;
                if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add( new MetadataBuilder(x));
            }
        }
        public TableFieldCheckConstraintBuilder(string name, string expression, IEnumerable<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            Name = name;
            Expression = expression;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
