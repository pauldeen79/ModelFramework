namespace DatabaseFramework.CodeGeneration.Models;

public interface ICheckConstraintContainer
{
    [Required] IReadOnlyCollection<ICheckConstraint> CheckConstraints { get; }
}
