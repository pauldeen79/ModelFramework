namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface ICheckConstraintContainer
{
    [Required] IReadOnlyCollection<ICheckConstraint> CheckConstraints { get; }
}
