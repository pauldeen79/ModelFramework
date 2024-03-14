namespace DatabaseFramework.CodeGeneration.Models;

internal interface IUniqueConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    [Required][MinCount(1)] IReadOnlyCollection<IUniqueConstraintField> Fields { get; }
}
