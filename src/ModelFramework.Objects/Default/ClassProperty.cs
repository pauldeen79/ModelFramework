using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    /// <summary>
    /// Default implementation for the IClassProperty interface.
    /// </summary>
    public class ClassProperty : IClassProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassProperty" /> class.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace
        /// or
        /// typeName;TypeName cannot be null or whitespace</exception>
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
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
            Attributes = new List<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            GetterCodeStatements = new List<ICodeStatement>(getterCodeStatements ?? Enumerable.Empty<ICodeStatement>());
            SetterCodeStatements = new List<ICodeStatement>(setterCodeStatements ?? Enumerable.Empty<ICodeStatement>());
            InitCodeStatements = new List<ICodeStatement>(initCodeStatements ?? Enumerable.Empty<ICodeStatement>());
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassField"/> is static.
        /// </summary>
        /// <value>
        ///   <c>true</c> if static; otherwise, <c>false</c>.
        /// </value>
        public bool Static { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has a getter.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has a getter; otherwise, <c>false</c>.
        /// </value>
        public bool HasGetter { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has a setter.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has a setter; otherwise, <c>false</c>.
        /// </value>
        public bool HasSetter { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has an initializer.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has an initializer; otherwise, <c>false</c>.
        /// </value>
        public bool HasInit { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }

        /// <summary>
        /// Gets the visibility.
        /// </summary>
        /// <value>
        /// The visibility.
        /// </value>
        public Visibility Visibility { get; }

        /// <summary>
        /// Gets the getter visibility.
        /// </summary>
        /// <value>
        /// The getter visibility.
        /// </value>
        public Visibility GetterVisibility { get; }

        /// <summary>
        /// Gets the setter visibility.
        /// </summary>
        /// <value>
        /// The setter visibility.
        /// </value>
        public Visibility SetterVisibility { get; }

        /// <summary>
        /// Gets the initializer visibility.
        /// </summary>
        /// <value>
        /// The initializer visibility.
        /// </value>
        public Visibility InitVisibility { get; }

        /// <summary>
        /// Gets the getter body.
        /// </summary>
        /// <value>
        /// The getter body.
        /// </value>
        public string GetterBody { get; }

        /// <summary>
        /// Gets the setter body.
        /// </summary>
        /// <value>
        /// The setter body.
        /// </value>
        public string SetterBody { get; }

        /// <summary>
        /// Gets the initializer body.
        /// </summary>
        /// <value>
        /// The initializer body.
        /// </value>
        public string InitBody { get; }

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
        /// Gets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public string TypeName { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassField"/> is virtual.
        /// </summary>
        /// <value>
        ///   <c>true</c> if virtual; otherwise, <c>false</c>.
        /// </value>
        public bool Virtual { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassField"/> is abstract.
        /// </summary>
        /// <value>
        ///   <c>true</c> if abstract; otherwise, <c>false</c>.
        /// </value>
        public bool Abstract { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassField"/> is protected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if protected; otherwise, <c>false</c>.
        /// </value>
        public bool Protected { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassField"/> is override.
        /// </summary>
        /// <value>
        ///   <c>true</c> if override; otherwise, <c>false</c>.
        /// </value>
        public bool Override { get; }

        /// <summary>
        /// Gets the name of the explicit interface.
        /// </summary>
        /// <value>
        /// The name of the explicit interface.
        /// </value>
        public string ExplicitInterfaceName { get; }

        /// <summary>
        /// Code statements for the get method.
        /// </summary>
        public IReadOnlyCollection<ICodeStatement> GetterCodeStatements { get; }

        /// <summary>
        /// Code statements for the set method.
        /// </summary>
        public IReadOnlyCollection<ICodeStatement> SetterCodeStatements { get; }

        /// <summary>
        /// Code statements for the initializer method.
        /// </summary>
        public IReadOnlyCollection<ICodeStatement> InitCodeStatements { get; }

        public override string ToString() => Name;
    }
}
