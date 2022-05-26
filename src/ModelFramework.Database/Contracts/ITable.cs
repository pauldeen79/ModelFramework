namespace ModelFramework.Database.Contracts;

public interface ITable : INameContainer, IMetadataContainer, IFileGroupNameContainer, ICheckConstraintContainer
{
    IReadOnlyCollection<IPrimaryKeyConstraint> PrimaryKeyConstraints { get; }
    IReadOnlyCollection<IUniqueConstraint> UniqueConstraints { get; }
    IReadOnlyCollection<IDefaultValueConstraint> DefaultValueConstraints { get; }
    IReadOnlyCollection<IForeignKeyConstraint> ForeignKeyConstraints { get; }
    IReadOnlyCollection<IIndex> Indexes { get; }
    IReadOnlyCollection<ITableField> Fields { get; }
}
