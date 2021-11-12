using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record IndexField : IIndexField
    {
        public IndexField(string name, bool isDescending = false, IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            IsDescending = isDescending;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public bool IsDescending { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
