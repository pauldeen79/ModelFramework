using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record Attribute : IAttribute
    {
        public Attribute(string name,
                         IEnumerable<IAttributeParameter>? parameters = null,
                         IEnumerable<IMetadata>? metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Parameters = new ValueCollection<IAttributeParameter>(parameters ?? Enumerable.Empty<IAttributeParameter>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public ValueCollection<IAttributeParameter> Parameters { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public string Name { get; }

        public override string ToString() => Name;
    }
}
