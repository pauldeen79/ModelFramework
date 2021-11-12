using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record ClassConstructor : IClassConstructor
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public ClassConstructor(Visibility visibility = Visibility.Public,
                                bool @static = false,
                                bool @virtual = false,
                                bool @abstract = false,
                                bool @protected = false,
                                bool @override = false,
                                string body = null,
                                string chainCall = null,
                                IEnumerable<IParameter> parameters = null,
                                IEnumerable<IAttribute> attributes = null,
                                IEnumerable<ICodeStatement> codeStatements = null,
                                IEnumerable<IMetadata> metadata = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Visibility = visibility;
            Static = @static;
            Virtual = @virtual;
            Abstract = @abstract;
            Protected = @protected;
            Override = @override;
            Body = body;
            ChainCall = chainCall;
            Parameters = new ValueCollection<IParameter>(parameters ?? Enumerable.Empty<IParameter>());
            Attributes = new ValueCollection<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            CodeStatements = new ValueCollection<ICodeStatement>(codeStatements ?? Enumerable.Empty<ICodeStatement>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }
        public ValueCollection<IMetadata> Metadata { get; }
        public bool Static { get; }
        public bool Virtual { get; }
        public bool Abstract { get; }
        public bool Protected { get; }
        public bool Override { get; }
        public Visibility Visibility { get; }
        public ValueCollection<IAttribute> Attributes { get; }
        public string Body { get; }
        public string ChainCall { get; }
        public ValueCollection<IParameter> Parameters { get; }
        public ValueCollection<ICodeStatement> CodeStatements { get; }
    }
}
