using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface ISchema : INameContainer, IMetadataContainer
    {
        ValueCollection<ITable> Tables { get; }
        ValueCollection<IStoredProcedure> StoredProcedures { get; }
        ValueCollection<IView> Views { get; }
    }
}
