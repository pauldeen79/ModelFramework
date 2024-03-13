namespace DatabaseFramework.CodeGeneration.Models;

internal interface ICheckConstraintContainer
{
    [Required] IReadOnlyCollection<ICheckConstraint> CheckConstraints { get; }
}
