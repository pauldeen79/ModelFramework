using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    public record Interface : IInterface
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public Interface(string name,
                         string @namespace,
                         Visibility visibility = Visibility.Public,
                         bool partial = false,
                         IEnumerable<string> interfaces = null,
                         IEnumerable<IClassProperty> properties = null,
                         IEnumerable<IClassMethod> methods = null,
                         IEnumerable<IMetadata> metadata = null,
                         IEnumerable<IAttribute> attributes = null,
                         IEnumerable<string> genericTypeArguments = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Namespace = @namespace;
            Visibility = visibility;
            Partial = partial;
            Interfaces = new ValueCollection<string>(interfaces ?? Enumerable.Empty<string>());
            Properties = new ValueCollection<IClassProperty>(properties ?? Enumerable.Empty<IClassProperty>());
            Methods = new ValueCollection<IClassMethod>(methods ?? Enumerable.Empty<IClassMethod>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
            Attributes = new ValueCollection<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            GenericTypeArguments = genericTypeArguments?.ToArray() ?? Array.Empty<string>();
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

        public string[] GenericTypeArguments { get; }

        public override string ToString() => !string.IsNullOrEmpty(Namespace) ? $"{Namespace}.{Name}" : Name;
    }
}
