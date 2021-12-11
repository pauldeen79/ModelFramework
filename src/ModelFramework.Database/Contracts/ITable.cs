using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface ITable : INameContainer, IMetadataContainer, IFileGroupNameContainer, ICheckConstraintContainer
    {
        ValueCollection<IPrimaryKeyConstraint> PrimaryKeyConstraints { get; }
        ValueCollection<IUniqueConstraint> UniqueConstraints { get; }
        ValueCollection<IDefaultValueConstraint> DefaultValueConstraints { get; }
        ValueCollection<IForeignKeyConstraint> ForeignKeyConstraints { get; }
        ValueCollection<IIndex> Indexes { get; }
        ValueCollection<ITableField> Fields { get; }
    }
}
