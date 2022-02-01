namespace ModelFramework.Database.Contracts;

public interface IForeignKeyConstraint : INameContainer, IMetadataContainer
{
    ValueCollection<IForeignKeyConstraintField> LocalFields { get; }
    ValueCollection<IForeignKeyConstraintField> ForeignFields { get; }
    string ForeignTableName { get; }
    CascadeAction CascadeUpdate { get; }
    CascadeAction CascadeDelete { get; }
}
