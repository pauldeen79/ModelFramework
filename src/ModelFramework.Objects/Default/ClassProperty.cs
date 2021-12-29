using System;
using System.Collections.Generic;
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
                             bool @static,
                             bool @virtual,
                             bool @abstract,
                             bool @protected,
                             bool @override,
                             bool hasGetter,
                             bool hasSetter,
                             bool hasInitializer,
                             bool isNullable,
                             Visibility visibility,
                             Visibility? getterVisibility,
                             Visibility? setterVisibility,
                             Visibility? initializerVisibility,
                             string explicitInterfaceName,
                             IEnumerable<IMetadata> metadata,
                             IEnumerable<IAttribute> attributes,
                             IEnumerable<ICodeStatement> getterCodeStatements,
                             IEnumerable<ICodeStatement> setterCodeStatements,
                             IEnumerable<ICodeStatement> initializerCodeStatements)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentOutOfRangeException(nameof(typeName), "TypeName cannot be null or whitespace");
            }

            Name = name;
            TypeName = typeName;
            Static = @static;
            Virtual = @virtual;
            Abstract = @abstract;
            Protected = @protected;
            Override = @override;
            HasGetter = hasGetter;
            HasSetter = hasSetter && !hasInitializer;
            HasInitializer = hasInitializer;
            IsNullable = isNullable;
            Visibility = visibility;
            GetterVisibility = getterVisibility;
            SetterVisibility = setterVisibility;
            InitializerVisibility = initializerVisibility;
            ExplicitInterfaceName = explicitInterfaceName;
            Metadata = new ValueCollection<IMetadata>(metadata);
            Attributes = new ValueCollection<IAttribute>(attributes);
            GetterCodeStatements = new ValueCollection<ICodeStatement>(getterCodeStatements);
            SetterCodeStatements = new ValueCollection<ICodeStatement>(setterCodeStatements);
            InitializerCodeStatements = new ValueCollection<ICodeStatement>(initializerCodeStatements);
        }

        public bool Static { get; }
        public bool HasGetter { get; }
        public bool HasSetter { get; }
        public bool HasInitializer { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public Visibility Visibility { get; }
        public Visibility? GetterVisibility { get; }
        public Visibility? SetterVisibility { get; }
        public Visibility? InitializerVisibility { get; }
        public string Name { get; }
        public ValueCollection<IAttribute> Attributes { get; }
        public string TypeName { get; }
        public bool Virtual { get; }
        public bool Abstract { get; }
        public bool Protected { get; }
        public bool Override { get; }
        public bool IsNullable { get; }
        public string ExplicitInterfaceName { get; }
        public ValueCollection<ICodeStatement> GetterCodeStatements { get; }
        public ValueCollection<ICodeStatement> SetterCodeStatements { get; }
        public ValueCollection<ICodeStatement> InitializerCodeStatements { get; }

        public override string ToString() => Name;
    }
}
