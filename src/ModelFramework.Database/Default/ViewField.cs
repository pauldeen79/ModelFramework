using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record ViewField : IViewField
    {
        public ViewField(string name,
                         string sourceSchemaName = null,
                         string sourceObjectName = null,
                         string expression = null,
                         string alias = null,
                         IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            SourceSchemaName = sourceSchemaName;
            SourceObjectName = sourceObjectName;
            Expression = expression;
            Alias = alias;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public string SourceSchemaName { get; }
        public string SourceObjectName { get; }
        public string Expression { get; }
        public string Alias { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
