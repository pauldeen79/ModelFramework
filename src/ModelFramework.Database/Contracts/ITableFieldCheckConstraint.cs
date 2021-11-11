using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface ITableFieldCheckConstraint : INameContainer, IMetadataContainer
    {
        string Expression { get; }
    }
}