using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record Index : IIndex
    {
        public Index(string name,
                     bool unique,
                     string fileGroupName,
                     IEnumerable<IIndexField> fields,
                     IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            if (!fields.Any())
            {
                throw new ArgumentException("Fields should contain at least 1 value", nameof(fields));
            }

            Name = name;
            Unique = unique;
            FileGroupName = fileGroupName;
            Fields = new ValueCollection<IIndexField>(fields);
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public ValueCollection<IIndexField> Fields { get; }
        public bool Unique { get; }
        public string Name { get; }
        public string FileGroupName { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
