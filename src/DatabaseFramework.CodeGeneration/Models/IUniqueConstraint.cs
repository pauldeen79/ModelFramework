namespace DatabaseFramework.CodeGeneration.Models;

internal interface IUniqueConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    [Required] IReadOnlyCollection<IUniqueConstraintField> Fields { get; }
}
