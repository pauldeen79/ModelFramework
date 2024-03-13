namespace DatabaseFramework.CodeGeneration.Models;

internal interface IPrimaryKeyConstraint : Abstractions.INameContainer, Abstractions.IMetadataContainer, IFileGroupNameContainer
{
    [Required] IReadOnlyCollection<IPrimaryKeyConstraintField> Fields { get; }
}
