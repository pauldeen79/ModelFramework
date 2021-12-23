using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record ViewCondition : IViewCondition
    {
        public ViewCondition(string expression,
                             string combination,
                             string fileGroupName = "",
                             IEnumerable<IMetadata>? metadata = null)
        {
            Combination = combination;
            Expression = expression;
            FileGroupName = fileGroupName;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public string Expression { get; }
        public string Combination { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public string FileGroupName { get; }
    }
}
