namespace DatabaseFramework.CodeGeneration.Models;

internal interface ITable : IDatabaseObject, IFileGroupNameContainer, ICheckConstraintContainer
{
    [Required] IReadOnlyCollection<IPrimaryKeyConstraint> PrimaryKeyConstraints { get; }
    [Required] IReadOnlyCollection<IUniqueConstraint> UniqueConstraints { get; }
    [Required] IReadOnlyCollection<IDefaultValueConstraint> DefaultValueConstraints { get; }
    [Required] IReadOnlyCollection<IForeignKeyConstraint> ForeignKeyConstraints { get; }
    [Required] IReadOnlyCollection<IIndex> Indexes { get; }
    [Required][MinCount(1)] IReadOnlyCollection<ITableField> Fields { get; }
}
