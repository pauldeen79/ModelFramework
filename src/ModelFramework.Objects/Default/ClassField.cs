using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    /// <summary>
    /// Default implementation for the IClassField interface.
    /// </summary>
    /// <seealso cref="ModelFramework.Contracts.IClassField" />
    public class ClassField : IClassField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassField" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="static">if set to <c>true</c> [static].</param>
        /// <param name="constant">if set to <c>true</c> [constant].</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="virtual">if set to <c>true</c> [virtual].</param>
        /// <param name="abstract">if set to <c>true</c> [abstract].</param>
        /// <param name="protected">if set to <c>true</c> [protected].</param>
        /// <param name="override">if set to <c>true</c> [override].</param>
        /// <param name="event">if set to <c>true</c> [event].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="attributes">The attributes.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace
        /// or
        /// typeName;TypeName cannot be null or whitespace</exception>
        public ClassField
        (
            string name,
            string typeName,
            bool @static = false,
            bool constant = false,
            bool readOnly = false,
            bool @virtual = false,
            bool @abstract = false,
            bool @protected = false,
            bool @override = false,
            bool @event = false,
            object defaultValue = null,
            Visibility visibility = Visibility.Private,
            IEnumerable<IMetadata> metadata = null,
            IEnumerable<IAttribute> attributes = null
        )
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
            DefaultValue = defaultValue;
            Visibility = visibility;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
            Attributes = new List<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassField"/> is static.
        /// </summary>
        /// <value>
        ///   <c>true</c> if static; otherwise, <c>false</c>.
        /// </value>
        public bool Static { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassField"/> is read-only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if read-only; otherwise, <c>false</c>.
        /// </value>
        public bool ReadOnly { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClassField"/> is constant.
        /// </summary>
        /// <value>
        ///   <c>true</c> if constant; otherwise, <c>false</c>.
        /// </value>
        public bool Constant { get; }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue { get; }

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
        /// Gets a value indicating whether this <see cref="ClassField"/> is an event.
        /// </summary>
        /// <value>
        ///   <c>true</c> if event; otherwise, <c>false</c>.
        /// </value>
        public bool Event { get; }

        public override string ToString() => Name;
    }
}
