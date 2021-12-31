using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record Interface : IInterface
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public Interface(string name,
                         string @namespace,
                         Visibility visibility,
                         bool partial,
                         IEnumerable<string> interfaces,
                         IEnumerable<IClassProperty> properties,
                         IEnumerable<IClassMethod> methods,
                         IEnumerable<IMetadata> metadata,
                         IEnumerable<IAttribute> attributes,
                         IEnumerable<string> genericTypeArguments)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Namespace = @namespace;
            Visibility = visibility;
            Partial = partial;
            Interfaces = new ValueCollection<string>(interfaces);
            Properties = new ValueCollection<IClassProperty>(properties);
            Methods = new ValueCollection<IClassMethod>(methods);
            Metadata = new ValueCollection<IMetadata>(metadata);
            Attributes = new ValueCollection<IAttribute>(attributes);
            GenericTypeArguments = new ValueCollection<string>(genericTypeArguments);
        }

        public string Namespace { get; }
        public bool Partial { get; }
        public ValueCollection<string> Interfaces { get; }
        public ValueCollection<IClassProperty> Properties { get; }
        public ValueCollection<IClassMethod> Methods { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public Visibility Visibility { get; }
        public string Name { get; }
        public ValueCollection<IAttribute> Attributes { get; }
        public ValueCollection<string> GenericTypeArguments { get; }

        public override string ToString()
            => !string.IsNullOrEmpty(Namespace)
                ? $"{Namespace}.{Name}"
                : Name;
    }
}
