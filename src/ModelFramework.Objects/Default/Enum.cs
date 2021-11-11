using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    /// <summary>
    /// Default implementation for the IEnum interface.
    /// </summary>
    public class Enum : IEnum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enum" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="members">The members.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="metadata">The metadata.</param>
        public Enum(string name
            , IEnumerable<IEnumMember> members
            , Visibility visibility = Visibility.Public
            , IEnumerable<IAttribute> attributes = null
            , IEnumerable<IMetadata> metadata = null)
        {
            Name = name;
            Visibility = visibility;
            Members = new List<IEnumMember>(members ?? Enumerable.Empty<IEnumMember>());
            Attributes = new List<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IReadOnlyCollection<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <value>
        /// The members.
        /// </value>
        public IReadOnlyCollection<IEnumMember> Members { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

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
        
        public override string ToString() => Name;
    }
}
