using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record EnumMember : IEnumMember
    {
        public EnumMember(string name, object? value = null,
                          IEnumerable<IAttribute>? attributes = null,
                          IEnumerable<IMetadata>? metadata = null)
        {
            Name = name;
            Value = value;
            Attributes = new ValueCollection<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public ValueCollection<IAttribute> Attributes { get; }
        public string Name { get; }
        public object? Value { get; }
        public ValueCollection<IMetadata> Metadata { get; }

        public override string ToString() => Value != null ? $"[{Name}] = [{Value}]" : Name;
    }
}
