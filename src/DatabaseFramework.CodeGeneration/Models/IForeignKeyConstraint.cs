namespace DatabaseFramework.CodeGeneration.Models;

internal interface IForeignKeyConstraint : INameContainer, IMetadataContainer
{
    [Required] IReadOnlyCollection<IForeignKeyConstraintField> LocalFields { get; }
    [Required] IReadOnlyCollection<IForeignKeyConstraintField> ForeignFields { get; }
    [Required] string ForeignTableName { get; }
    [Required] CascadeAction CascadeUpdate { get; }
    [Required] CascadeAction CascadeDelete { get; }
}
