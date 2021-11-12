using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Default
{
    public record Enum : IEnum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enum" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="members">The members.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="metadata">The metadata.</param>
        public Enum(string name,
                    IEnumerable<IEnumMember> members,
                    Visibility visibility = Visibility.Public,
                    IEnumerable<IAttribute> attributes = null,
                    IEnumerable<IMetadata> metadata = null)
        {
            Name = name;
            Visibility = visibility;
            Members = new ValueCollection<IEnumMember>(members ?? Enumerable.Empty<IEnumMember>());
            Attributes = new ValueCollection<IAttribute>(attributes ?? Enumerable.Empty<IAttribute>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public ValueCollection<IAttribute> Attributes { get; }
        public ValueCollection<IEnumMember> Members { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public Visibility Visibility { get; }
        
        public override string ToString() => Name;
    }
}
