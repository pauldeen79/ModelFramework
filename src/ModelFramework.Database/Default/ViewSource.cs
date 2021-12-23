using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record ViewSource : IViewSource
    {
        public ViewSource(string name,
                          string sourceObjectName,
                          string sourceSchemaName,
                          string alias,
                          IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Alias = alias;
            SourceSchemaName = sourceSchemaName;
            SourceObjectName = sourceObjectName;
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public string Alias { get; }
        public string Name { get; }
        public string SourceSchemaName { get; }
        public string SourceObjectName { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
