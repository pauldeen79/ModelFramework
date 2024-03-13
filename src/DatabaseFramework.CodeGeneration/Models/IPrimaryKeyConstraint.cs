namespace DatabaseFramework.CodeGeneration.Models;

public interface IPrimaryKeyConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    [Required] IReadOnlyCollection<IPrimaryKeyConstraintField> Fields { get; }
}
