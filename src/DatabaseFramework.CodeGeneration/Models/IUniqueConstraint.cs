namespace DatabaseFramework.CodeGeneration.Models;

internal interface IUniqueConstraint : INameContainer, IFileGroupNameContainer
{
    [Required][MinCount(1)] IReadOnlyCollection<IUniqueConstraintField> Fields { get; }
}
