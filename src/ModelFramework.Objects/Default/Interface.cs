using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    /// <summary>
    /// Default implementation for the IInterface interface.
    /// </summary>
    public class Interface : IInterface
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Interface" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="namespace">The namespace.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="partial">if set to <c>true</c> [partial].</param>
        /// <param name="interfaces">The interfaces.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="methods">The methods.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="attributes">The attributes.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
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
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Namespace = @namespace;
            Visibility = visibility;
            Partial = partial;
            Interfaces = new List<string>(interfaces ?? Enumerable.Empty<string>());
            Properties = new List<IClassProperty>(properties ?? Enumerable.Empty<IClassProperty>());
            Methods = new List<IClassMethod>(methods ?? Enumerable.Empty<IClassMethod>());
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
            Attributes = new List<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            GenericTypeArguments = genericTypeArguments?.ToArray() ?? Array.Empty<string>();
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <value>
        /// The namespace.
        /// </value>
        public string Namespace { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Class"/> is partial.
        /// </summary>
        /// <value>
        ///   <c>true</c> if partial; otherwise, <c>false</c>.
        /// </value>
        public bool Partial { get; }

        /// <summary>
        /// Gets the interfaces.
        /// </summary>
        /// <value>
        /// The interfaces.
        /// </value>
        public IReadOnlyCollection<string> Interfaces { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public IReadOnlyCollection<IClassProperty> Properties { get; }

        /// <summary>
        /// Gets the methods.
        /// </summary>
        /// <value>
        /// The methods.
        /// </value>
        public IReadOnlyCollection<IClassMethod> Methods { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }

        /// <summary>
        /// Gets the visibility.
        /// </summary>
        /// <value>
        /// The visibility.
        /// </value>
        public Visibility Visibility { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IReadOnlyCollection<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the generic type arguments.
        /// </summary>
        public string[] GenericTypeArguments { get; }

        public override string ToString() => !string.IsNullOrEmpty(Namespace) ? $"{Namespace}.{Name}" : Name;
    }
}
