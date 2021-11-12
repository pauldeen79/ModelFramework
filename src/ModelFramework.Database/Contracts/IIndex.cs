using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IIndex : INameContainer, IMetadataContainer, IFileGroupNameContainer
    {
        ValueCollection<IIndexField> Fields { get; }
        bool Unique { get; }
    }
}
