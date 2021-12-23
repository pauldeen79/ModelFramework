using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record ViewOrderByField : IViewOrderByField
    {
        public ViewOrderByField(string name,
                                string sourceSchemaName = "",
                                string sourceObjectName = "",
                                string expression = "",
                                string alias = "",
                                bool descending = false,
                                IEnumerable<IMetadata>? metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            SourceSchemaName = sourceSchemaName;
            SourceObjectName = sourceObjectName;
            Expression = expression;
            Alias = alias;
            Descending = descending;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public bool Descending { get; }
        public string SourceSchemaName { get; }
        public string SourceObjectName { get; }
        public string Expression { get; }
        public string Alias { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
