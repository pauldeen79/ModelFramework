using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record AttributeParameter : IAttributeParameter
    {
        public AttributeParameter(object value,
                                 string name = "",
                                 IEnumerable<IMetadata>? metadata = null)
        {
            Name = name;
            Value = value ?? throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be null");
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public object Value { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public string Name { get; }

        public override string ToString() => $"{Name.WhenNullOrEmpty(Value.ToStringWithDefault(string.Empty))}";
    }
}
