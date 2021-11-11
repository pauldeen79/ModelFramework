using System.Collections.Generic;

namespace ModelFramework.Common.Contracts
{
    public interface IMetadataContainer
    {
        IReadOnlyCollection<IMetadata> Metadata { get; }
    }
}
