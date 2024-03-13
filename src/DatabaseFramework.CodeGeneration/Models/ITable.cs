namespace DatabaseFramework.CodeGeneration.Models;

internal interface ITable : Abstractions.INameContainer, Abstractions.IMetadataContainer, IFileGroupNameContainer, ICheckConstraintContainer
{
    [Required] IReadOnlyCollection<IPrimaryKeyConstraint> PrimaryKeyConstraints { get; }
    [Required] IReadOnlyCollection<IUniqueConstraint> UniqueConstraints { get; }
    [Required] IReadOnlyCollection<IDefaultValueConstraint> DefaultValueConstraints { get; }
    [Required] IReadOnlyCollection<IForeignKeyConstraint> ForeignKeyConstraints { get; }
    [Required] IReadOnlyCollection<IIndex> Indexes { get; }
    [Required] IReadOnlyCollection<ITableField> Fields { get; }
}
