namespace DatabaseFramework.CodeGeneration.Models;

internal interface IPrimaryKeyConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    [Required] IReadOnlyCollection<IPrimaryKeyConstraintField> Fields { get; }
}
