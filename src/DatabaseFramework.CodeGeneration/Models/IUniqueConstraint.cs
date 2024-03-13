namespace DatabaseFramework.CodeGeneration.Models;

internal interface IUniqueConstraint : Abstractions.INameContainer, Abstractions.IMetadataContainer, IFileGroupNameContainer
{
    [Required] IReadOnlyCollection<IUniqueConstraintField> Fields { get; }
}
