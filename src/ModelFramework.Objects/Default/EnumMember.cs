using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record EnumMember : IEnumMember
    {
        public EnumMember(string name,
                          object? value,
                          IEnumerable<IAttribute> attributes,
                          IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Value = value;
            Attributes = new ValueCollection<IAttribute>(attributes);
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public ValueCollection<IAttribute> Attributes { get; }
        public string Name { get; }
        public object? Value { get; }
        public ValueCollection<IMetadata> Metadata { get; }

        public override string ToString()
            => Value != null
                ? $"[{Name}] = [{Value}]"
                : Name;
    }
}
