namespace ModelFramework.Database.Contracts;

public interface IForeignKeyConstraint : INameContainer, IMetadataContainer
{
    IReadOnlyCollection<IForeignKeyConstraintField> LocalFields { get; }
    IReadOnlyCollection<IForeignKeyConstraintField> ForeignFields { get; }
    string ForeignTableName { get; }
    CascadeAction CascadeUpdate { get; }
    CascadeAction CascadeDelete { get; }
}
