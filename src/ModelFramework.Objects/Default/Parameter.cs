using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record Parameter : IParameter
    {
        public Parameter(string name,
                         string typeName,
                         object? defaultValue,
                         bool isNullable,
                         bool isParamArray,
                         IEnumerable<IAttribute> attributes,
                         IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentOutOfRangeException(nameof(typeName), "Name cannot be null or whitespace");
            }

            Name = name;
            TypeName = typeName;
            DefaultValue = defaultValue;
            IsNullable = isNullable;
            IsParamArray = isParamArray;
            Attributes = new ValueCollection<IAttribute>(attributes);
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public string TypeName { get; }
        public ValueCollection<IAttribute> Attributes { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public string Name { get; }
        public object? DefaultValue { get; }
        public bool IsNullable { get; }
        public bool IsParamArray { get; }

        public override string ToString() => Name;
    }
}
