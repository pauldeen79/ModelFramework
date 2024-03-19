namespace DatabaseFramework.CodeGeneration.Models;

internal interface IPrimaryKeyConstraint : INameContainer, IFileGroupNameContainer
{
    [Required][MinCount(1)] IReadOnlyCollection<IPrimaryKeyConstraintField> Fields { get; }
}
