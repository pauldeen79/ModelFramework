using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class CheckConstraintBuilder
    {
        public string Expression { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public ICheckConstraint Build()
        {
            return new CheckConstraint(Name, Expression, Metadata.Select(x => x.Build()));
        }
        public CheckConstraintBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }
        public CheckConstraintBuilder Clear()
        {
            Expression = string.Empty;
            Name = string.Empty;
            Metadata.Clear();
            return this;
        }
        public CheckConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public CheckConstraintBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public CheckConstraintBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public CheckConstraintBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            foreach (var itemToAdd in metadata)
            {
                Metadata.Add(itemToAdd);
            }
            return this;
        }
        public CheckConstraintBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public CheckConstraintBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public CheckConstraintBuilder()
        {
            Expression = string.Empty;
            Name = string.Empty;
            Metadata = new List<MetadataBuilder>();
        }
        public CheckConstraintBuilder(ICheckConstraint source)
        {
            Metadata = new List<MetadataBuilder>();

            Expression = source.Expression;
            Name = source.Name;
            foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
        }
    }
}
