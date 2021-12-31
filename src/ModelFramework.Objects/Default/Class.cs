using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record Class : IClass
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public Class(string name,
                     string @namespace,
                     Visibility visibility,
                     string baseClass,
                     bool @static,
                     bool @sealed,
                     bool partial,
                     bool record,
                     IEnumerable<string> interfaces,
                     IEnumerable<IClassField> fields,
                     IEnumerable<IClassProperty> properties,
                     IEnumerable<IClassMethod> methods,
                     IEnumerable<IClassConstructor> constructors,
                     IEnumerable<IMetadata> metadata,
                     IEnumerable<IAttribute> attributes,
                     IEnumerable<IClass> subClasses,
                     IEnumerable<IEnum> enums,
                     IEnumerable<string> genericTypeArguments)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Namespace = @namespace;
            BaseClass = baseClass;
            Visibility = visibility;
            Static = @static;
            Sealed = @sealed;
            Partial = partial;
            Record = record;
            Interfaces = new ValueCollection<string>(interfaces);
            Fields = new ValueCollection<IClassField>(fields);
            Properties = new ValueCollection<IClassProperty>(properties);
            Methods = new ValueCollection<IClassMethod>(methods);
            Constructors = new ValueCollection<IClassConstructor>(constructors);
            Attributes = new ValueCollection<IAttribute>(attributes);
            Metadata = new ValueCollection<IMetadata>(metadata);
            SubClasses = new ValueCollection<IClass>(subClasses);
            Enums = new ValueCollection<IEnum>(enums);
            GenericTypeArguments = new ValueCollection<string>(genericTypeArguments);
        }

        public ValueCollection<string> Interfaces { get; }
        public ValueCollection<IClassField> Fields  { get; }
        public ValueCollection<IClassProperty> Properties  { get; }
        public bool Static  { get; }
        public bool Sealed  { get; }
        public bool Partial { get; }
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
        public ValueCollection<string> GenericTypeArguments { get; }

        public override string ToString()
            => !string.IsNullOrEmpty(Namespace)
                ? $"{Namespace}.{Name}"
                : Name;
    }
}
