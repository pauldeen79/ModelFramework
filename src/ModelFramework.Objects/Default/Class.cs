using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    public class Class : IClass
    {
        public Class(string name,
                     string @namespace,
                     Visibility visibility = Visibility.Public,
                     string baseClass = null,
                     bool @static = false,
                     bool @sealed = false,
                     bool partial = false,
                     bool autoGenerateInterface = false,
                     bool record = false,
                     IEnumerable<string> interfaces = null,
                     IEnumerable<IClassField> fields = null,
                     IEnumerable<IClassProperty> properties = null,
                     IEnumerable<IClassMethod> methods = null,
                     IEnumerable<IClassConstructor> constructors = null,
                     IEnumerable<IMetadata> metadata = null,
                     IEnumerable<IAttribute> attributes = null,
                     IEnumerable<IClass> subClasses = null,
                     IEnumerable<IEnum> enums = null,
                     IEnumerable<string> genericTypeArguments = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Namespace = @namespace;
            BaseClass = baseClass;
            Visibility = visibility;
            Static = @static;
            Sealed = @sealed;
            Partial = partial;
            AutoGenerateInterface = autoGenerateInterface;
            Record = record;
            Interfaces = new List<string>(interfaces ?? Enumerable.Empty<string>());
            Fields = new List<IClassField>(fields ?? Enumerable.Empty<IClassField>());
            Properties = new List<IClassProperty>(properties ?? Enumerable.Empty<IClassProperty>());
            Methods = new List<IClassMethod>(methods ?? Enumerable.Empty<IClassMethod>());
            Constructors = new List<IClassConstructor>(constructors ?? Enumerable.Empty<IClassConstructor>());
            Attributes = new List<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
            SubClasses = new List<IClass>(subClasses ?? Enumerable.Empty<IClass>());
            Enums = new List<IEnum>(enums ?? Enumerable.Empty<IEnum>());
            GenericTypeArguments = genericTypeArguments?.ToArray() ?? Array.Empty<string>();
        }

        public IReadOnlyCollection<string> Interfaces { get; }
        public IReadOnlyCollection<IClassField> Fields  { get; }
        public IReadOnlyCollection<IClassProperty> Properties  { get; }
        public bool Static  { get; }
        public bool Sealed  { get; }
        public bool Partial { get; }
        public bool AutoGenerateInterface { get; }
        public bool Record { get; }
        public IReadOnlyCollection<IMetadata> Metadata  { get; }
        public Visibility Visibility  { get; }
        public string Name  { get; }
        public IReadOnlyCollection<IAttribute> Attributes  { get; }
        public IReadOnlyCollection<IClass> SubClasses { get; }
        public IReadOnlyCollection<IEnum> Enums { get; }
        public string Namespace { get; }
        public IReadOnlyCollection<IClassConstructor> Constructors { get; }
        public IReadOnlyCollection<IClassMethod> Methods { get; }
        public string BaseClass { get; }
        public string[] GenericTypeArguments { get; }

        public override string ToString() => !string.IsNullOrEmpty(Namespace) ? $"{Namespace}.{Name}" : Name;
    }
}
