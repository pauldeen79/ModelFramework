using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Settings;

namespace ModelFramework.Objects.Default
{
    public record Class : IClass
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public Class(string name,
                     string @namespace,
                     Visibility visibility = Visibility.Public,
                     string baseClass = null,
                     bool @static = false,
                     bool @sealed = false,
                     bool partial = false,
                     bool autoGenerateInterface = false,
                     InterfaceSettings autoGenerateInterfaceSettings = null,
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
#pragma warning restore S107 // Methods should not have too many parameters
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
            AutoGenerateInterfaceSettings = autoGenerateInterfaceSettings;
            Record = record;
            Interfaces = new ValueCollection<string>(interfaces ?? Enumerable.Empty<string>());
            Fields = new ValueCollection<IClassField>(fields ?? Enumerable.Empty<IClassField>());
            Properties = new ValueCollection<IClassProperty>(properties ?? Enumerable.Empty<IClassProperty>());
            Methods = new ValueCollection<IClassMethod>(methods ?? Enumerable.Empty<IClassMethod>());
            Constructors = new ValueCollection<IClassConstructor>(constructors ?? Enumerable.Empty<IClassConstructor>());
            Attributes = new ValueCollection<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
            SubClasses = new ValueCollection<IClass>(subClasses ?? Enumerable.Empty<IClass>());
            Enums = new ValueCollection<IEnum>(enums ?? Enumerable.Empty<IEnum>());
            GenericTypeArguments = genericTypeArguments?.ToArray() ?? Array.Empty<string>();
        }

        public ValueCollection<string> Interfaces { get; }
        public ValueCollection<IClassField> Fields  { get; }
        public ValueCollection<IClassProperty> Properties  { get; }
        public bool Static  { get; }
        public bool Sealed  { get; }
        public bool Partial { get; }
        public bool AutoGenerateInterface { get; }
        public InterfaceSettings AutoGenerateInterfaceSettings { get; }
        public bool Record { get; }
        public ValueCollection<IMetadata> Metadata  { get; }
        public Visibility Visibility  { get; }
        public string Name  { get; }
        public ValueCollection<IAttribute> Attributes  { get; }
        public ValueCollection<IClass> SubClasses { get; }
        public ValueCollection<IEnum> Enums { get; }
        public string Namespace { get; }
        public ValueCollection<IClassConstructor> Constructors { get; }
        public ValueCollection<IClassMethod> Methods { get; }
        public string BaseClass { get; }
        public string[] GenericTypeArguments { get; }

        public override string ToString() => !string.IsNullOrEmpty(Namespace) ? $"{Namespace}.{Name}" : Name;
    }
}
