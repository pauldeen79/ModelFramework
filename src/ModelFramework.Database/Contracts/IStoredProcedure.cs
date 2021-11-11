using System.Collections.Generic;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IStoredProcedure : INameContainer, IMetadataContainer
    {
        string Body { get; }
        IReadOnlyCollection<IStoredProcedureParameter> Parameters { get; }
    }
}
