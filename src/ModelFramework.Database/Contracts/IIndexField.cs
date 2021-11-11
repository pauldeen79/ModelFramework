using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IIndexField : INameContainer, IMetadataContainer
    {
        bool IsDescending { get; }
    }
}
