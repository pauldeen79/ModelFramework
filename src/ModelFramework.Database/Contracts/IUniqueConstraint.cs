namespace ModelFramework.Database.Contracts;

public interface IUniqueConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    IReadOnlyCollection<IUniqueConstraintField> Fields { get; }
}
