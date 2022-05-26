namespace ModelFramework.Database.Contracts;

public interface IPrimaryKeyConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    IReadOnlyCollection<IPrimaryKeyConstraintField> Fields { get; }
}
