namespace DatabaseFramework.CodeGeneration.Models;

public interface IUniqueConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    [Required] IReadOnlyCollection<IUniqueConstraintField> Fields { get; }
}
