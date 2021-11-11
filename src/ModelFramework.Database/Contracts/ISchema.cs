using System.Collections.Generic;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface ISchema : INameContainer, IMetadataContainer
    {
        IReadOnlyCollection<ITable> Tables { get; }
        IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; }
        IReadOnlyCollection<IView> Views { get; }
    }
}
