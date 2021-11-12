using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IStoredProcedure : INameContainer, IMetadataContainer
    {
        string Body { get; }
        ValueCollection<ISqlStatement> Statements { get; }
        ValueCollection<IStoredProcedureParameter> Parameters { get; }
    }
}
