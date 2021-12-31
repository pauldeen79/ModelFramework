using System.Collections.Generic;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record AttributeParameter : IAttributeParameter
    {
        public AttributeParameter(object value,
                                  string name,
                                  IEnumerable<IMetadata> metadata)
        {
            Name = name;
            Value = value;
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public object Value { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public string Name { get; }

        public override string ToString() => $"{Name.WhenNullOrEmpty(() => Value.ToStringWithDefault())}";
    }
}
