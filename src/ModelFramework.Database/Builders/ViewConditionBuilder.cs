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
            return new ViewCondition(Combination, Expression, FileGroupName, Metadata.Select(x => x.Build()));
        }
        public ViewConditionBuilder Clear()
        {
            Expression = default;
            Combination = default;
            Metadata.Clear();
            FileGroupName = default;
            return this;
        }
        public ViewConditionBuilder Update(ViewCondition source)
        {
            Expression = default;
            Combination = default;
            Metadata = new List<MetadataBuilder>();
            FileGroupName = default;
            if (source != null)
            {
                Expression = source.Expression;
                Combination = source.Combination;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                FileGroupName = source.FileGroupName;
            }
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
            if (metadata != null)
            {
                foreach (var itemToAdd in metadata)
                {
                    Metadata.Add(itemToAdd);
                }
            }
            return this;
        }
        public ViewConditionBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewConditionBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ViewConditionBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }
        public ViewConditionBuilder(IViewCondition source = null)
        {
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Expression = source.Expression;
                Combination = source.Combination;
                if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
                FileGroupName = source.FileGroupName;
            }
        }
        public ViewConditionBuilder(string combination,
                                    string expression,
                                    string fileGroupName = null,
                                    IEnumerable<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            Expression = expression;
            Combination = combination;
            FileGroupName = fileGroupName;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
