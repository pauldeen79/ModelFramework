using System.Collections.Generic;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IIndex : INameContainer, IMetadataContainer, IFileGroupNameContainer
    {
        IReadOnlyCollection<IIndexField> Fields { get; }
        bool Unique { get; }
    }
}
