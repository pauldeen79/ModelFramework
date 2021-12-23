using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record ClassConstructor : IClassConstructor
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public ClassConstructor(Visibility visibility,
                                bool @static,
                                bool @virtual,
                                bool @abstract,
                                bool @protected,
                                bool @override,
                                string chainCall,
                                IEnumerable<IParameter> parameters,
                                IEnumerable<IAttribute> attributes,
                                IEnumerable<ICodeStatement> codeStatements,
                                IEnumerable<IMetadata> metadata)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Visibility = visibility;
            Static = @static;
            Virtual = @virtual;
            Abstract = @abstract;
            Protected = @protected;
            Override = @override;
            ChainCall = chainCall;
            Parameters = new ValueCollection<IParameter>(parameters);
            Attributes = new ValueCollection<IAttribute>(attributes);
            CodeStatements = new ValueCollection<ICodeStatement>(codeStatements);
            Metadata = new ValueCollection<IMetadata>(metadata);
        }
        public ValueCollection<IMetadata> Metadata { get; }
        public bool Static { get; }
        public bool Virtual { get; }
        public bool Abstract { get; }
        public bool Protected { get; }
        public bool Override { get; }
        public Visibility Visibility { get; }
        public ValueCollection<IAttribute> Attributes { get; }
        public string ChainCall { get; }
        public ValueCollection<IParameter> Parameters { get; }
        public ValueCollection<ICodeStatement> CodeStatements { get; }
    }
}
