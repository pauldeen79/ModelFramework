using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record Parameter : IParameter
    {
        public Parameter(string name, string typeName, object defaultValue = null, IEnumerable<IAttribute> attributes = null, IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentOutOfRangeException(nameof(typeName), "Name cannot be null or whitespace");
            Name = name;
            TypeName = typeName;
            DefaultValue = defaultValue;
            Attributes = new ValueCollection<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public string TypeName { get; }
        public ValueCollection<IAttribute> Attributes { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public string Name { get; }
        public object DefaultValue { get; }

        public override string ToString() => Name;
    }
}
