using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class ViewConditionBuilder
    {
        public string Expression { get; set; }
        public string Combination { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public string FileGroupName { get; set; }
        public IViewCondition Build()
        {
            return new ViewCondition(Expression,
                                     Combination,
                                     FileGroupName,
                                     Metadata.Select(x => x.Build()));
        }
        public ViewConditionBuilder Clear()
        {
            Expression = string.Empty;
            Combination = string.Empty;
            Metadata.Clear();
            FileGroupName = string.Empty;
            return this;
        }
        public ViewConditionBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }
        public ViewConditionBuilder WithCombination(string combination)
        {
            Combination = combination;
            return this;
        }
        public ViewConditionBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ViewConditionBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewConditionBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            foreach (var itemToAdd in metadata)
            {
                Metadata.Add(itemToAdd);
            }
            return this;
        }
        public ViewConditionBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewConditionBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public ViewConditionBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }
        public ViewConditionBuilder()
        {
            Expression = string.Empty;
            FileGroupName = string.Empty;
            Combination = string.Empty;
            Metadata = new List<MetadataBuilder>();
        }
        public ViewConditionBuilder(IViewCondition source)
        {
            Metadata = new List<MetadataBuilder>();

            Expression = source.Expression;
            Combination = source.Combination;
            foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            FileGroupName = source.FileGroupName;
        }
    }
}
