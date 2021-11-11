using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IStoredProcedureParameter : INameContainer, IMetadataContainer
    {
        string Type { get; }
        string DefaultValue { get; }
    }
}
