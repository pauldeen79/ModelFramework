using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record ClassMethod : IClassMethod
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public ClassMethod(string name,
                           string typeName,
                           Visibility visibility,
                           bool @static,
                           bool @virtual,
                           bool @abstract,
                           bool @protected,
                           bool partial,
                           bool @override,
                           bool extensionMethod,
                           bool @operator,
                           bool isNullable,
                           string explicitInterfaceName,
                           IEnumerable<IParameter> parameters,
                           IEnumerable<IAttribute> attributes,
                           IEnumerable<ICodeStatement> codeStatements,
                           IEnumerable<IMetadata> metadata)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            TypeName = typeName;
            Visibility = visibility;
            Static = @static;
            Virtual = @virtual;
            Abstract = @abstract;
            Protected = @protected;
            Partial = partial;
            Override = @override;
            ExtensionMethod = extensionMethod;
            Operator = @operator;
            IsNullable = isNullable;
            ExplicitInterfaceName = explicitInterfaceName;
            Parameters = new ValueCollection<IParameter>(parameters);
            Attributes = new ValueCollection<IAttribute>(attributes);
            CodeStatements = new ValueCollection<ICodeStatement>(codeStatements);
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public bool Partial { get; }
        public bool ExtensionMethod { get; }
        public bool Operator { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public bool Static { get; }
        public bool Virtual { get; }
        public bool Abstract{ get; }
        public bool Protected { get; }
        public bool Override { get; }
        public bool IsNullable { get; }
        public Visibility Visibility { get; }
        public string Name { get; }
        public ValueCollection<IAttribute> Attributes { get; }
        public ValueCollection<IParameter> Parameters { get; }
        public string TypeName { get; }
        public string ExplicitInterfaceName { get; }
        public ValueCollection<ICodeStatement> CodeStatements { get; }

        public override string ToString() => Name;
    }
}
