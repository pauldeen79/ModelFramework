using System.Collections.Generic;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IUniqueConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
    {
        IReadOnlyCollection<IUniqueConstraintField> Fields { get; }
    }
}
