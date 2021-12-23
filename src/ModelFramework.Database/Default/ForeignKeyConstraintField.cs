using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record ForeignKeyConstraintField : IForeignKeyConstraintField
    {
        public ForeignKeyConstraintField(string name, IEnumerable<IMetadata>? metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
