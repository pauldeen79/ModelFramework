using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    public class ClassMethod : IClassMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassMethod" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="static">if set to <c>true</c> [static].</param>
        /// <param name="virtual">if set to <c>true</c> [virtual].</param>
        /// <param name="abstract">if set to <c>true</c> [abstract].</param>
        /// <param name="protected">if set to <c>true</c> [protected].</param>
        /// <param name="partial">if set to <c>true</c> [partial].</param>
        /// <param name="override">if set to <c>true</c> [override].</param>
        /// <param name="extensionMethod">if set to <c>true</c> [extension method].</param>
        /// <param name="operator">if set to <c>true</c> [operator]</param>
        /// <param name="body">The body.</param>
        /// <param name="explicitInterfaceName">Name of the explicit interface.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="codeStatements">The code statements.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        public ClassMethod(string name
            , string typeName
            , Visibility visibility = Visibility.Public
            , bool @static = false
            , bool @virtual = false
            , bool @abstract = false
            , bool @protected = false
            , bool partial = false
            , bool @override = false
            , bool extensionMethod = false
            , bool @operator = false
            , string body = null
            , string explicitInterfaceName = null
            , IEnumerable<IParameter> parameters = null
            , IEnumerable<IAttribute> attributes = null
            , IEnumerable<ICodeStatement> codeStatements = null
            , IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            //if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentOutOfRangeException(nameof(typeName), "TypeName cannot be null or whitespace");

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
            Body = body;
            ExplicitInterfaceName = explicitInterfaceName;
            Parameters = new List<IParameter>(parameters ?? Enumerable.Empty<IParameter>());
            Attributes = new List<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            CodeStatements = new List<ICodeStatement>(codeStatements ?? Enumerable.Empty<ICodeStatement>());
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassMethod"/> is partial.
        /// </summary>
        /// <value>
        ///   <c>true</c> if partial; otherwise, <c>false</c>.
        /// </value>
        public bool Partial { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassMethod"/> is an extension method.
        /// </summary>
        /// <value>
        ///   <c>true</c> if extension method; otherwise, <c>false</c>.
        /// </value>
        public bool ExtensionMethod { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassMethod"/> is an operator method.
        /// </summary>
        /// <value>
        ///   <c>true</c> if operator method; otherwise, <c>false</c>.
        /// </value>
        public bool Operator { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassMethod"/> is static.
        /// </summary>
        /// <value>
        ///   <c>true</c> if static; otherwise, <c>false</c>.
        /// </value>
        public bool Static { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassMethod"/> is virtual.
        /// </summary>
        /// <value>
        ///   <c>true</c> if virtual; otherwise, <c>false</c>.
        /// </value>
        public bool Virtual { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassMethod"/> is abstract.
        /// </summary>
        /// <value>
        ///   <c>true</c> if abstract; otherwise, <c>false</c>.
        /// </value>
        public bool Abstract{ get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassMethod"/> is protected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if protected; otherwise, <c>false</c>.
        /// </value>
        public bool Protected { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassMethod"/> is override.
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
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

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
        public string Body{ get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IReadOnlyCollection<IParameter> Parameters { get; }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public string TypeName { get; }

        /// <summary>
        /// Gets the name of the explicit interface.
        /// </summary>
        /// <value>
        /// The name of the explicit interface.
        /// </value>
        public string ExplicitInterfaceName { get; }

        /// <summary>
        /// Gets the code stateents
        /// </summary>
        /// <value>
        /// The code statements.
        /// </value>
        public IReadOnlyCollection<ICodeStatement> CodeStatements { get; }

        public override string ToString() => Name;
    }
}
