using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    /// <summary>
    /// Default implementation for the IClassConstructor interface.
    /// </summary>
    public class ClassConstructor : IClassConstructor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassConstructor" /> class.
        /// </summary>
        /// <param name="visibility">The visibility.</param>
        /// <param name="static">if set to <c>true</c> [static].</param>
        /// <param name="virtual">if set to <c>true</c> [virtual].</param>
        /// <param name="abstract">if set to <c>true</c> [abstract].</param>
        /// <param name="protected">if set to <c>true</c> [protected].</param>
        /// <param name="override">if set to <c>true</c> [override].</param>
        /// <param name="body">The body.</param>
        /// <param name="chainCall">The chain call.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="codeStatements">The code statements.</param>
        /// <param name="metadata">The metadata.</param>
        public ClassConstructor(Visibility visibility = Visibility.Public
            , bool @static = false
            , bool @virtual = false
            , bool @abstract = false
            , bool @protected = false
            , bool @override = false
            , string body = null
            , string chainCall = null
            , IEnumerable<IParameter> parameters = null
            , IEnumerable<IAttribute> attributes = null
            , IEnumerable<ICodeStatement> codeStatements = null
            , IEnumerable<IMetadata> metadata = null)
        {
            Visibility = visibility;
            Static = @static;
            Virtual = @virtual;
            Abstract = @abstract;
            Protected = @protected;
            Override = @override;
            Body = body;
            ChainCall = chainCall;
            Parameters = new List<IParameter>(parameters ?? Enumerable.Empty<IParameter>());
            Attributes = new List<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            CodeStatements = new List<ICodeStatement>(codeStatements ?? Enumerable.Empty<ICodeStatement>());
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassConstructor"/> is static.
        /// </summary>
        /// <value>
        ///   <c>true</c> if static; otherwise, <c>false</c>.
        /// </value>
        public bool Static { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassConstructor"/> is virtual.
        /// </summary>
        /// <value>
        ///   <c>true</c> if virtual; otherwise, <c>false</c>.
        /// </value>
        public bool Virtual { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassConstructor"/> is abstract.
        /// </summary>
        /// <value>
        ///   <c>true</c> if abstract; otherwise, <c>false</c>.
        /// </value>
        public bool Abstract { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassConstructor"/> is protected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if protected; otherwise, <c>false</c>.
        /// </value>
        public bool Protected { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassConstructor"/> is override.
        /// </summary>
        /// <value>
        ///   <c>true</c> if override; otherwise, <c>false</c>.
        /// </value>
        public bool Override { get; }

        /// <summary>
        /// Gets the visibility.
        /// </summary>
        /// <value>
        /// The visibility.
        /// </value>
        public Visibility Visibility { get; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IReadOnlyCollection<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; }

        /// <summary>
        /// Gets the chain call.
        /// </summary>
        /// <value>
        /// The chain call.
        /// </value>
        public string ChainCall { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IReadOnlyCollection<IParameter> Parameters { get; }

        /// <summary>
        /// Gets the code stateents
        /// </summary>
        /// <value>
        /// The code statements.
        /// </value>
        public IReadOnlyCollection<ICodeStatement> CodeStatements { get; }
    }
}
