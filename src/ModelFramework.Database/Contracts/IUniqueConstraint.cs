namespace ModelFramework.Database.Contracts;

public interface IUniqueConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    ValueCollection<IUniqueConstraintField> Fields { get; }
}
