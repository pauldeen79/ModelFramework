namespace ModelFramework.Database.Contracts;

public interface ICheckConstraintContainer
{
    IReadOnlyCollection<ICheckConstraint> CheckConstraints { get; }
}
