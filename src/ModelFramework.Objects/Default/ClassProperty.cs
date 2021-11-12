using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record ClassProperty : IClassProperty
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public ClassProperty(string name,
                             string typeName,
                             bool @static = false,
                             bool @virtual = false,
                             bool @abstract = false,
                             bool @protected = false,
                             bool @override = false,
                             bool hasGetter = true,
                             bool hasSetter = true,
                             bool hasInit = false,
                             Visibility visibility = Visibility.Public,
                             Visibility getterVisibility = Visibility.Public,
                             Visibility setterVisibility = Visibility.Public,
                             Visibility initVisibility = Visibility.Public,
                             string getterBody = null,
                             string setterBody = null,
                             string initBody = null,
                             string explicitInterfaceName = null,
                             IEnumerable<IMetadata> metadata = null,
                             IEnumerable<IAttribute> attributes = null,
                             IEnumerable<ICodeStatement> getterCodeStatements = null,
                             IEnumerable<ICodeStatement> setterCodeStatements = null,
                             IEnumerable<ICodeStatement> initCodeStatements = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentOutOfRangeException(nameof(typeName), "TypeName cannot be null or whitespace");

            Name = name;
            TypeName = typeName;
            Static = @static;
            Virtual = @virtual;
            Abstract = @abstract;
            Protected = @protected;
            Override = @override;
            HasGetter = hasGetter;
            HasSetter = hasSetter && !hasInit;
            HasInit = hasInit;
            Visibility = visibility;
            GetterVisibility = getterVisibility;
            SetterVisibility = setterVisibility;
            InitVisibility = initVisibility;
            GetterBody = getterBody;
            SetterBody = setterBody;
            InitBody = initBody;
            ExplicitInterfaceName = explicitInterfaceName;
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
            Attributes = new ValueCollection<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            GetterCodeStatements = new ValueCollection<ICodeStatement>(getterCodeStatements ?? Enumerable.Empty<ICodeStatement>());
            SetterCodeStatements = new ValueCollection<ICodeStatement>(setterCodeStatements ?? Enumerable.Empty<ICodeStatement>());
            InitCodeStatements = new ValueCollection<ICodeStatement>(initCodeStatements ?? Enumerable.Empty<ICodeStatement>());
        }

        public bool Static { get; }
        public bool HasGetter { get; }
        public bool HasSetter { get; }
        public bool HasInit { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public Visibility Visibility { get; }
        public Visibility GetterVisibility { get; }
        public Visibility SetterVisibility { get; }
        public Visibility InitVisibility { get; }
        public string GetterBody { get; }
        public string SetterBody { get; }
        public string InitBody { get; }
        public string Name { get; }
        public ValueCollection<IAttribute> Attributes { get; }
        public string TypeName { get; }
        public bool Virtual { get; }
        public bool Abstract { get; }
        public bool Protected { get; }
        public bool Override { get; }
        public string ExplicitInterfaceName { get; }
        public ValueCollection<ICodeStatement> GetterCodeStatements { get; }
        public ValueCollection<ICodeStatement> SetterCodeStatements { get; }
        public ValueCollection<ICodeStatement> InitCodeStatements { get; }

        public override string ToString() => Name;
    }
}
