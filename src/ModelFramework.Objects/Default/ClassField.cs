using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record ClassField : IClassField
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public ClassField(string name,
                          string typeName,
                          bool @static = false,
                          bool constant = false,
                          bool readOnly = false,
                          bool @virtual = false,
                          bool @abstract = false,
                          bool @protected = false,
                          bool @override = false,
                          bool @event = false,
                          bool isNullable = false,
                          object defaultValue = null,
                          Visibility visibility = Visibility.Private,
                          IEnumerable<IMetadata> metadata = null,
                          IEnumerable<IAttribute> attributes = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentOutOfRangeException(nameof(typeName), "TypeName cannot be null or whitespace");

            Name = name;
            TypeName = typeName;
            Static = @static;
            Constant = constant;
            ReadOnly = readOnly;
            Virtual = @virtual;
            Abstract = @abstract;
            Protected = @protected;
            Override = @override;
            Event = @event;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
            Visibility = visibility;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
            Attributes = new ValueCollection<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
        }

        public bool Static { get; }
        public bool ReadOnly { get; }
        public bool Constant { get; }
        public object DefaultValue { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public Visibility Visibility { get; }
        public string Name { get; }
        public ValueCollection<IAttribute> Attributes { get; }
        public string TypeName { get; }
        public bool Virtual { get; }
        public bool Abstract { get; }
        public bool Protected { get; }
        public bool Override { get; }
        public bool Event { get; }
        public bool IsNullable { get; }

        public override string ToString() => Name;
    }
}
